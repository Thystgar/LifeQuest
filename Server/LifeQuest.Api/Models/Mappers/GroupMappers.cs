using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Models.Mappers
{
    public static class GrouptMappers
    {
        public static GroupApiModel ToApiModel(this GroupStorageModel storageModel)
        {
            return new GroupApiModel
            {
                Id = storageModel.Id,
                Name = storageModel.Name,
                Description = storageModel.Description
            };
        }

        public static GroupStorageModel ToStorageModel(this GroupApiModel apiModel)
        {
            return new GroupStorageModel
            {
                Id = apiModel.Id,
                Name = apiModel.Name,
                Description = apiModel.Description
            };
        }
    }
}