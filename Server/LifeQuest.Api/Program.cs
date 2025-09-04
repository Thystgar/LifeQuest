using LifeQuest.Api.Processors;
using LifeQuest.Api.Storage;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Identity;
using OpenTelemetry.Trace;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

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
                .UseKestrel();
            return builder;
        }
    }
}
