using LifeQuest.Api.Processors;
using LifeQuest.Api.Storage;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Identity;
using OpenTelemetry.Trace;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Azure.Security.KeyVault.Secrets;
using System.Security.Cryptography.X509Certificates;

namespace LifeQuest.Api
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args);

            var app = builder.Build();

            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("LifeQuest API is starting up.");

            app.Run();
            
            app.Services.GetRequiredService<ILogger<Program>>()
                .LogInformation("LifeQuest API has started successfully.");
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args){
            var builder = WebHost.CreateDefaultBuilder();

            builder
                .ConfigureAppConfiguration((context, config) =>
                {
                    var env = context.HostingEnvironment.EnvironmentName;
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetValue<string>("Database:ConnectionString");
                    var azureMonitorConnectionString = context.Configuration.GetValue<string>("AzureMonitor:ConnectionString");

                    services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });
                    services.AddDbContext<LifeQuestContext>(options => options.UseSqlServer(connectionString));
                    services.AddScoped<IQuestStorage, QuestDbStorage>();
                    services.AddScoped<IRewardStorage, RewardDbStorage>();
                    services.AddScoped<IAccountStorage, AccountDbStorage>();
                    services.AddScoped<IAccountProcessor, AccountProcessor>();
                    services.AddScoped<IRewardProcessor, RewardProcessor>();
                    services.AddScoped<IQuestProcessor, QuestProcessor>();
                    services.AddScoped<IUserContext, UserContext>();
                    services.AddScoped<IGroupStorage, GroupDbStorage>();
                    services.AddScoped<IGroupProcessor, GroupProcessor>();
                    services.AddLogging();

                    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();
                    services.AddOpenTelemetry()
                        .WithTracing(builder =>
                        {
                            builder
                                .AddAspNetCoreInstrumentation()
                                .AddHttpClientInstrumentation();
                                // .AddSqlClientInstrumentation(); // Uncomment if you want SQL telemetry
                        })
                        .WithLogging()
                        .UseAzureMonitor(options =>
                        {
                            options.Credential = new DefaultAzureCredential();
                            options.ConnectionString = azureMonitorConnectionString;
                        });
                    services
                        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApi(context.Configuration.GetSection("AzureAd"));

                    services.AddAuthorization();

                })
                .Configure((context, app) =>
                {
                    var env = context.HostingEnvironment;

                    if (env.IsDevelopment())
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                    }

                    app.UseHttpsRedirection();
                    app.UseRouting();

                    app.UseAuthentication();
                    app.UseAuthorization();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                })
                .UseKestrel((context, options) =>
                {
                    var kvUri = context.Configuration.GetValue<string>("KeyVault:Url");
                    var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                    var logger = loggerFactory.CreateLogger<Program>();

                    if (string.IsNullOrEmpty(kvUri))
                    {
                        logger.LogWarning("Key Vault URL is not configured. HTTPS with certificate from Key Vault will not be enabled.");
                        return;
                    }

                    logger.LogInformation("Attempting to retrieve TLS certificate from Key Vault at {KeyVaultUrl}", kvUri);
                    try
                    {
                        var secretClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
                        var certSecret = secretClient.GetSecret("lifequestwebsite-tls");
                        
                        if (certSecret?.Value?.Value == null)
                        {
                            logger.LogError("TLS certificate secret 'lifequestwebsite-tls' not found in Key Vault or has no value");
                            return;
                        }

                        var certBytes = Convert.FromBase64String(certSecret.Value.Value.ToString());
                            var certificate = X509CertificateLoader.LoadPkcs12(certBytes, null);
                        
                        logger.LogInformation("Successfully loaded TLS certificate from Key Vault. Configuring HTTPS endpoint.");
                            options.ListenAnyIP(443, listenOptions =>
                            {
                                listenOptions.UseHttps(certificate);
                            });
                        logger.LogInformation("HTTPS endpoint configured successfully on port 443");
                        }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to configure HTTPS endpoint with certificate from Key Vault");
                        throw; // Re-throw the exception to prevent startup with invalid certificate
                    }
                });
            return builder;
        }
    }
}
