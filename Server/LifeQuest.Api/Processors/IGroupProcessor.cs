using LifeQuest.Api.Models.API;

namespace LifeQuest.Api.Processors
{
    public interface IGroupProcessor
    {
        Task<GroupApiModel> GetGroupByIdAsync(string groupId);
        Task<string> AddGroupAsync(NewGroupApiModel group);
        Task UpdateGroupAsync(GroupApiModel group);
        Task<GroupApiModel> GetGroupByInviteCodeAsync(string inviteCode);
        Task DeleteGroupAsync(string groupId);
    }
}
