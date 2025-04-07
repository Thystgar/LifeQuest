using LifeQuest.Api.Models.Storage;
using Microsoft.EntityFrameworkCore;

namespace LifeQuest.Api.Storage
{
    public class QuestDbStorage : IQuestStorage
    {
        private readonly LifeQuestContext _context;

        public QuestDbStorage(LifeQuestContext context)
        {
            _context = context;
        }

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
    }
}