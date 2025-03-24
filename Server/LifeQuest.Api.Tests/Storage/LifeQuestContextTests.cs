using LifeQuest.Api.Models;
using LifeQuest.Api;
using LifeQuest.Api.Storage;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Tests.Storage
{
    public class LifeQuestContextTests
    {
        [Fact]
        public void CanInstantiateDbContext()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddDbContext<LifeQuestContext>(options => options.UseSqlServer(@"Server=localhost; Database=LifeQest; User=sa; Password=P@ssw0rd!;"));
            var app = builder.Build();

            var lifeQuestContext = app.Services.GetService<LifeQuestContext>();

            lifeQuestContext.Should().NotBeNull();
            lifeQuestContext.Accounts.Should().NotBeNull();
            lifeQuestContext.Quests.Should().NotBeNull();
            lifeQuestContext.Rewards.Should().NotBeNull();
        }
        
        [Fact]
        public async Task LifeQuestContext_Accounts_CanRetrieveAccounts()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddDbContext<LifeQuestContext>(options => options.UseSqlServer(@"Server=localhost; Database=LifeQest; User=sa; Password=P@ssw0rd!;"));
            var app = builder.Build();

            var lifeQuestContext = app.Services.GetRequiredService<LifeQuestContext>();

            await lifeQuestContext.Accounts.AddAsync(new AccountStorageModel { Name = "TestAccount", Points = 100 });

            var account = lifeQuestContext.Accounts.FirstOrDefault(a => a.Name == "TestAccount");
            account.Should().NotBeNull();
        }
    }
}