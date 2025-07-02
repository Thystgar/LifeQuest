using LifeQuest.Api.Processors;
using LifeQuest.Api.Storage;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Identity;

namespace LifeQuest.Api
{
    public partial class Program
    {
        public static void Main( string[] args )
        {
            Console.WriteLine("[Main] Starting LifeQuest API host builder...");
            var builder = CreateHostBuilder(args);

            Console.WriteLine("[Main] Building application...");
            var app = builder.Build();

            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("LifeQuest API is starting up.");
            Console.WriteLine("[Main] LifeQuest API is starting up.");

            app.Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args){
            Console.WriteLine("[CreateHostBuilder] Creating default web host builder...");
            var builder = WebHost.CreateDefaultBuilder();

            builder
                .ConfigureAppConfiguration((context, config) =>
                {
                    var env = context.HostingEnvironment.EnvironmentName;
                    Console.WriteLine($"[ConfigureAppConfiguration] Environment: {env}");
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    Console.WriteLine("[ConfigureAppConfiguration] Configuration loaded.");
                })
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetValue<string>("Database:ConnectionString");
                    Console.WriteLine($"[ConfigureServices] Using DB connection string: {connectionString}");

                    services.AddControllers();
                    services.AddDbContext<LifeQuestContext>(options => options.UseSqlServer(connectionString));
                    services.AddScoped<IQuestStorage, QuestDbStorage>();
                    services.AddScoped<IRewardStorage, RewardDbStorage>();
                    services.AddScoped<IAccountStorage, AccountDbStorage>();
                    services.AddScoped<IAccountProcessor, AccountProcessor>();
                    services.AddScoped<IRewardProcessor, RewardProcessor>();
                    services.AddScoped<IQuestProcessor, QuestProcessor>();
                    services.AddLogging();

                    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();
                    services.AddOpenTelemetry().UseAzureMonitor(options =>
                    {
                        options.Credential = new DefaultAzureCredential();
                        options.ConnectionString = "InstrumentationKey=ad65f563-d9d6-47b3-bb77-2a32b9a42cca;IngestionEndpoint=https://northeurope-2.in.applicationinsights.azure.com/;LiveEndpoint=https://northeurope.livediagnostics.monitor.azure.com/;ApplicationId=5dcab56d-7fa7-4385-b7ee-7af600a775fd";
                    });

                    Console.WriteLine("[ConfigureServices] Services configured.");
                    services.BuildServiceProvider().GetRequiredService<ILogger<Program>>()
                        .LogInformation("LifeQuest API is starting up.");
                })
                .Configure((context, app) =>
                {
                    var env = context.HostingEnvironment;
                    Console.WriteLine($"[Configure] Configuring middleware for environment {env}...");

                    app.ApplicationServices.GetRequiredService<ILogger<Program>>()
                        .LogInformation($"Configuring LifeQuest API middleware for environment {env}.");

                    if (env.IsDevelopment())
                    {
                        Console.WriteLine("[Configure] Enabling Swagger for development environment.");
                        app.UseSwagger();
                        app.UseSwaggerUI();
                    }

                    app.UseHttpsRedirection();
                    app.UseRouting();
                    app.UseAuthorization();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });

                    app.ApplicationServices.GetRequiredService<ILogger<Program>>()
                        .LogInformation($"Configured LifeQuest API middleware for environment {env}.");
                    Console.WriteLine($"[Configure] Middleware configured for environment {env}.");

                })
                .UseKestrel();
            Console.WriteLine("[CreateHostBuilder] Web host builder configured.");
            return builder;
        }
    }
}
