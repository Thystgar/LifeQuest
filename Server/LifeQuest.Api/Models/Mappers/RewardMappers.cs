using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Models.Mappers
{
    public static class RewardMappers
    {
        public static RewardApiModel ToApiModel(this RewardStorageModel storageModel)
        {
            return new RewardApiModel
            {
                Id = storageModel.Id,
                Name = storageModel.Name,
                Description = storageModel.Description,
                Value = storageModel.Value,
                Redeemed = storageModel.Redeemed,
                GroupId = storageModel.GroupId
            };
        }

        public static RewardStorageModel ToStorageModel(this RewardApiModel apiModel)
        {
            return new RewardStorageModel
            {
                Id = apiModel.Id,
                Name = apiModel.Name,
                Description = apiModel.Description,
                Value = apiModel.Value,
                Redeemed = apiModel.Redeemed,
                GroupId = apiModel.GroupId
            };
        }
    }
}