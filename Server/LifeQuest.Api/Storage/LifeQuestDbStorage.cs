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

        #region Quest Operations

        public async Task<IEnumerable<QuestStorageModel>> GetQuestsAsync()
        {
            return await _context.Quests.ToListAsync();
        }

        public async Task<QuestStorageModel?> GetQuestByIdAsync(string id)
        {
            return await _context.Quests.SingleOrDefaultAsync(q => q.Id == id);
        }

        public async Task AddQuestAsync(QuestStorageModel quest)
        {
            await _context.Quests.AddAsync(quest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestAsync(string id)
        {
            await _context.Quests.Where(q => q.Id == id).ExecuteDeleteAsync();
        }

        public async Task UpdateQuestAsync(QuestStorageModel quest)
        {
            await _context.Quests
                .Where(q => q.Id == quest.Id)
                .ExecuteUpdateAsync(q => q
                    .SetProperty(q => q.Name, quest.Name)
                    .SetProperty(q => q.Description, quest.Description)
                    .SetProperty(q => q.Status, quest.Status)
                    .SetProperty(q => q.Value, quest.Value));
        }

        #endregion

        #region Reward Operations

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

        #endregion

        #region Account Operations

        public async Task<AccountStorageModel?> GetAccountByIdAsync(string id)
        {
            return await _context.Accounts.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAccountAsync(AccountStorageModel account)
        {
            await _context.Accounts
                .Where(a => a.Id == account.Id)
                .ExecuteUpdateAsync(a => a  
                    .SetProperty(a => a.Name, account.Name)
                    .SetProperty(a => a.Points, account.Points));
        }

        #endregion
    }
}
