using LifeQuest.Api.Models.API;

namespace LifeQuest.Api.Processors
{
    public interface IGroupProcessor
    {
        Task<IEnumerable<GroupApiModel>> GetGroupsAsync();
        Task<GroupApiModel> GetGroupByIdAsync(string id);
        Task AddGroupAsync(GroupApiModel group);
        Task UpdateGroupAsync(GroupApiModel group);
    }
}
