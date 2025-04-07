using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Storage;

namespace LifeQuest.Api.Processors
{
    public class QuestProcessor
    {
        private readonly IQuestStorage _storage;
        private readonly AccountProcessor _account;
        public QuestProcessor(IQuestStorage storage, AccountProcessor account)
        {
            _storage = storage;
            _account = account;
        }

        public async Task<IEnumerable<QuestApiModel>> GetQuestsAsync() 
        {
            var quests = await _storage.GetQuestsAsync();
            return quests.Select(q => q.ToApiModel());
        }

        public async Task CompleteQuestAsync(string userId, string questId) 
        {
            var quests = await _storage.GetQuestsAsync();
            _account.AddPointsAsync(userId, quests.FirstOrDefault(q => q.Id == questId).Value);
        }
    }
}
