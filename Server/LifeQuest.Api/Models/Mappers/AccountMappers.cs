using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Models.Mappers
{
    public static class AccountMappers
    {
        public static AccountApiModel ToApiModel(this AccountStorageModel storageModel)
        {
            return new AccountApiModel
            {
                Id = storageModel.Id,
                Name = storageModel.Name,
                Points = storageModel.Points
            };
        }

        public static AccountStorageModel ToStorageModel(this AccountApiModel apiModel)
        {
            return new AccountStorageModel
            {
                Id = apiModel.Id,
                Name = apiModel.Name,
                Points = apiModel.Points
            };
        }
    }
}