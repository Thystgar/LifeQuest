using LifeQuest.Api.Models.Storage;
using Microsoft.EntityFrameworkCore;

namespace LifeQuest.Api.Storage
{
    public class LifeQuestDbStorage : ILifeQuestStorage
    {
        private readonly LifeQuestContext _context;

        public LifeQuestDbStorage(LifeQuestContext lifeQuestContext)
        {
            _context = lifeQuestContext;
        }

        public async Task<IEnumerable<QuestStorageModel>> GetQuestsAsync()
        {
            var quests = await _context.Quests.ToListAsync();
            return quests;
        }

        public async Task DeleteQuestAsync(string id) 
        {
            await _context.Quests.Where(q=>q.Id==id).ExecuteDeleteAsync();
        }

        public Task<QuestStorageModel> GetQuestByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RewardStorageModel>> GetRewardsAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteRewardAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task AddRewardAsync(RewardStorageModel reward)
        {
            await _context.Rewards.AddAsync(reward);
            await _context.SaveChangesAsync();
        }
    }
}
