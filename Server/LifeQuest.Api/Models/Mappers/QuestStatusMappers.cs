using System;
using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Models.Mappers
{
    public static class QuestStatusMappers
    {
        public static QuestStatusApiEnum ToApiModel(this QuestStatusStorageEnum storageModel)
        {
            return storageModel switch
            {
                QuestStatusStorageEnum.Accepted => QuestStatusApiEnum.Accepted,
                QuestStatusStorageEnum.Active => QuestStatusApiEnum.Active,
                QuestStatusStorageEnum.Completed => QuestStatusApiEnum.Completed,
                _ => throw new InvalidOperationException($"Invalid quest status: {storageModel}")
            };
        }

        public static QuestStatusStorageEnum ToStorageModel(this QuestStatusApiEnum apiModel)
        {
            return apiModel switch
            {
                QuestStatusApiEnum.Accepted => QuestStatusStorageEnum.Accepted,
                QuestStatusApiEnum.Active => QuestStatusStorageEnum.Active,
                QuestStatusApiEnum.Completed => QuestStatusStorageEnum.Completed,
                _ => throw new InvalidOperationException($"Invalid quest status: {apiModel}")
            };
        }
    }
}