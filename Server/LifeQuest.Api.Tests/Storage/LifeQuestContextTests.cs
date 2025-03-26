using LifeQuest.Api.Models.Storage;
using LifeQuest.Api.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LifeQuest.Api.Tests.Storage
{
	public class LifeQuestContextTests
	{
		[Fact]
		public void CanInstantiateDbContext()
		{
			var builder = WebApplication.CreateBuilder();
			builder.Services.AddDbContext<LifeQuestContext>( options => options.UseSqlServer( "Data Source=127.0.0.1;Initial Catalog=LifeQuest;User ID=sa;Password=P@ssw0rd!;Encrypt=False;Trust Server Certificate=True" ) );
			var app = builder.Build();

			var lifeQuestContext = app.Services.GetService<LifeQuestContext>();

			Assert.NotNull( lifeQuestContext );
			Assert.NotNull( lifeQuestContext.Accounts );
			Assert.NotNull( lifeQuestContext.Quests );
			Assert.NotNull( lifeQuestContext.Rewards );
		}

		[Fact]
		public async Task LifeQuestContext_AccountsLocal_CanConnectAndRetrieveAccounts()
		{
			var builder = WebApplication.CreateBuilder();
			builder.Services.AddDbContext<LifeQuestContext>( options => options.UseSqlServer( "Data Source=127.0.0.1;Initial Catalog=life-quest-db;User ID=sa;Password=P@ssw0rd!;Encrypt=False;Trust Server Certificate=True" ) );
			var app = builder.Build();

			using( var lifeQuestContext = app.Services.GetRequiredService<LifeQuestContext>() )
			{
				await lifeQuestContext.Accounts.AddAsync( new AccountStorageModel { Id = "id", Name = "testAccount", Points = 100 } );
				/// https://learn.microsoft.com/en-us/ef/core/saving/
				await lifeQuestContext.SaveChangesAsync();
				var account = await lifeQuestContext.Accounts.SingleAsync( a => a.Id == "id" );
				Assert.NotNull( account );
			}
		}

		[Fact]
		public async Task LifeQuestContext_AccountsAzure_CanConnectAndRetrieveAccounts()
		{
			var builder = WebApplication.CreateBuilder();
			builder.Services.AddDbContext<LifeQuestContext>( options => options.UseSqlServer( "Server=life-quest-db-server.database.windows.net;Database=life-quest-db;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;" ) );
			var app = builder.Build();

			using( var lifeQuestContext = app.Services.GetRequiredService<LifeQuestContext>() )
			{
				await lifeQuestContext.Accounts.AddAsync( new AccountStorageModel { Id = "id", Name = "testAccount", Points = 100 } );
				/// https://learn.microsoft.com/en-us/ef/core/saving/
				await lifeQuestContext.SaveChangesAsync();
				var account = await lifeQuestContext.Accounts.SingleAsync( a => a.Id == "id" );
				Assert.NotNull( account );
			}
		}

		private LifeQuestContext CreateLifeQuestContext()
		{
			var optionsBuilder = new DbContextOptionsBuilder<LifeQuestContext>();
			optionsBuilder.UseSqlServer( "Data Source=127.0.0.1;Initial Catalog=LifeQuest;User ID=sa;Password=P@ssw0rd!;Encrypt=False;Trust Server Certificate=True" );
			var lifeQuestContext = new LifeQuestContext( optionsBuilder.Options );
			return lifeQuestContext;
		}
	}
}