using LifeQuest.Api.Models.API;

namespace LifeQuest.Api.Processors
{
    public interface IGroupProcessor
    {
        Task<GroupApiModel> GetGroupByIdAsync(string id);
        Task AddGroupAsync(NewGroupApiModel group);
        Task UpdateGroupAsync(GroupApiModel group);
    }
}
