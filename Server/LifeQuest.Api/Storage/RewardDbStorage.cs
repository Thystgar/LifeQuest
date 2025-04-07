using LifeQuest.Api.Models.Storage;
using Microsoft.EntityFrameworkCore;

namespace LifeQuest.Api.Storage
{
    public class RewardDbStorage : IRewardStorage
    {
        private readonly LifeQuestContext _context;

        public RewardDbStorage(LifeQuestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RewardStorageModel>> GetRewardsAsync()
        {
            return await _context.Rewards.ToListAsync();
        }

        public async Task<RewardStorageModel?> GetRewardByIdAsync(string id)
        {
            return await _context.Rewards.SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddRewardAsync(RewardStorageModel reward)
        {
            await _context.Rewards.AddAsync(reward);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRewardAsync(string id)
        {
            await _context.Rewards.Where(r => r.Id == id).ExecuteDeleteAsync();
        }

        public async Task UpdateRewardAsync(RewardStorageModel reward)
        {
            await _context.Rewards
                .Where(r => r.Id == reward.Id)
                .ExecuteUpdateAsync(r => r
                    .SetProperty(r => r.Name, reward.Name)
                    .SetProperty(r => r.Description, reward.Description)
                    .SetProperty(r => r.Value, reward.Value)
                    .SetProperty(r => r.Redeemed, reward.Redeemed));
        }
    }
}