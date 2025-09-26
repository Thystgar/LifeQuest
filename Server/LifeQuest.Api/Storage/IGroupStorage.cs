using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Storage
{
    public interface IGroupStorage
    {
        Task<IEnumerable<GroupStorageModel>> GetGroupAsync();
        Task<GroupStorageModel?> GetGroupByIdAsync(string id);
        Task UpdateGroupAsync(GroupStorageModel group);
    }
}
