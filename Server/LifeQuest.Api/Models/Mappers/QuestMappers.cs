using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Models.Mappers
{
    public static class QuestMappers
    {
        public static QuestApiModel ToApiModel(this QuestStorageModel storageModel)
        {
            return new QuestApiModel
            {
                Name = storageModel.Name,
                Description = storageModel.Description,
                Value = storageModel.Value,
                Status = storageModel.Status.ToApiModel()
            };
        }

        public static QuestStorageModel ToStorageModel(this QuestApiModel apiModel)
        {
            return new QuestStorageModel
            {
                Name = apiModel.Name,
                Description = apiModel.Description,
                Value = apiModel.Value,
                Status = apiModel.Status.ToStorageModel()
            };
        }
    }
}