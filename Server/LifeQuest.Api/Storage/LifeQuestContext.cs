using LifeQuest.Api.Models.Storage;
using Microsoft.EntityFrameworkCore;

namespace LifeQuest.Api.Storage
{
    public class LifeQuestContext : DbContext
    {
        public DbSet<AccountStorageModel> Accounts { get; set; }
        public DbSet<QuestStorageModel> Quests { get; set; }
        public DbSet<RewardStorageModel> Rewards { get; set; }

        public LifeQuestContext(DbContextOptions<LifeQuestContext> options) : base(options) { }
    }
}
