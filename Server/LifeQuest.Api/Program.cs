using LifeQuest.Api.Processors;
using LifeQuest.Api.Storage;
using Microsoft.EntityFrameworkCore;

namespace LifeQuest.Api
{
	public class Program
	{
		public static void Main( string[] args )
		{
			var builder = WebApplication.CreateBuilder();

			// Add services to the container.
			RegisterServices(builder.Services, builder.Configuration);


			var app = builder.Build();

			ConfigureApplication(app);

			app.Run();
		}

		public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetValue<string>("Database:ConnectionString");

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
		}

		public static WebApplication ConfigureApplication(WebApplication app)
		{
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();

			return app;
		}
	}
}
