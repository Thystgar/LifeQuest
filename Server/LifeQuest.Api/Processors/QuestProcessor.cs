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
        private readonly IGroupProcessor _group;
        public QuestProcessor(IQuestStorage storage, IAccountProcessor account, IGroupProcessor groupProcessor)
        {
            _storage = storage;
            _account = account;
            _group = groupProcessor;
        }

        public async Task<IEnumerable<QuestApiModel>> GetQuestsAsync() 
        {
            var quests = await _storage.GetQuestsAsync();
            return quests.Select(q => q.ToApiModel());
        }

        public async Task<QuestApiModel> CompleteQuestAsync(string userId, string questId) 
        {
            //TODO: complete quest, at this moment all quests are repeatable
            var quest = await _storage.GetQuestByIdAsync(questId);
            await _account.AddPointsAsync(userId, quest.Value);
            return quest.ToApiModel();
        }

        public async Task AddQuestAsync(NewQuestApiModel quest)
        {
            var group = await _group.GetGroupAsync(); 
            var storageQuest = new QuestStorageModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = quest.Name,
                Description = quest.Description,
                Value = quest.Value,
                Status = QuestStatusStorageEnum.Active,
                GroupId = group.First().Id
            };

            await _storage.AddQuestAsync(storageQuest);
        }
    }
}
