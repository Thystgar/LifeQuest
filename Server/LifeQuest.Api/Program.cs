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
			builder.Services.AddControllers();
			builder.Services.AddDbContext<LifeQuestContext>( options => options.UseSqlServer( "Data Source=127.0.0.1;Initial Catalog=life-quest-db;User ID=sa;Password=P@ssw0rd!;Encrypt=False;Trust Server Certificate=True" ) );
			builder.Services.AddSingleton<RewardProcessor>();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if( app.Environment.IsDevelopment() )
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
