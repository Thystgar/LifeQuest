using LifeQuest.Api.Processors;
using LifeQuest.Api.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LifeQuest.Api
{
	public partial class Program
	{
		public static void Main( string[] args )
		{
            var builder = CreateHostBuilder(args);

			var app = builder.Build();

			app.Run();
		}

        public static IHostBuilder CreateHostBuilder(string[] args){
            var builder = new HostBuilder();

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

                    services.AddControllers();
                    services.AddDbContext<LifeQuestContext>(options => options.UseSqlServer(connectionString));
                    services.AddScoped<IQuestStorage, QuestDbStorage>();
                    services.AddScoped<IRewardStorage, RewardDbStorage>();
                    services.AddScoped<IAccountStorage, AccountDbStorage>();
                    services.AddScoped<IAccountProcessor, AccountProcessor>();
                    services.AddScoped<IRewardProcessor, RewardProcessor>();
                    services.AddScoped<IQuestProcessor, QuestProcessor>();

                    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();
                })
                .ConfigureWebHost(builder =>
                {
                    builder
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
                        app.UseAuthorization();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    })
                    .UseKestrel();
                });
                
            return builder;
        }
    }
}
