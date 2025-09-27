using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Models.Storage;
using LifeQuest.Api.Storage;

namespace LifeQuest.Api.Processors
{
    public class QuestProcessor : IQuestProcessor
    {
        private readonly IQuestStorage _storage;
        private readonly IAccountProcessor _account;

        public QuestProcessor(IQuestStorage storage, IAccountProcessor account)
        {
            _storage = storage;
            _account = account;
        }

        public async Task<IEnumerable<QuestApiModel>> GetQuestsAsync() 
        {
            var account = await _account.GetMyAccountAsync() ?? throw new NullReferenceException("Account not returned");
            var quests = await _storage.GetQuestsAsync();
            quests = quests.Where(q => q.GroupId == account.GroupId);
            return quests.Select(q => q.ToApiModel());
        }

        public async Task<QuestApiModel> CompleteQuestAsync(string userId, string questId) 
        {
            //TODO: complete quest, at this moment all quests are repeatable
            var quest = await _storage.GetQuestByIdAsync(questId) ?? throw new NullReferenceException("Quest not returned");
            await _account.AddPointsAsync(userId, quest.Value);
            return quest.ToApiModel();
        }

        public async Task AddQuestAsync(NewQuestApiModel quest)
        {
            var account = await _account.GetMyAccountAsync() ?? throw new NullReferenceException("Account not returned");
            var storageQuest = new QuestStorageModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = quest.Name,
                Description = quest.Description,
                Value = quest.Value,
                Status = QuestStatusStorageEnum.Active,
                GroupId = account.GroupId
            };

            await _storage.AddQuestAsync(storageQuest);
        }
    }
}
