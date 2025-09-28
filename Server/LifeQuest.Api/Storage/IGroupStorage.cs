using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Storage
{
    public interface IGroupStorage
    {
        Task<IEnumerable<GroupStorageModel>> GetGroupsAsync();
        Task<GroupStorageModel?> GetGroupByIdAsync(string id);
        Task UpdateGroupAsync(GroupStorageModel group);
        Task<GroupStorageModel?> GetGroupByInviteCodeAsync(string inviteCode);
    }
}
