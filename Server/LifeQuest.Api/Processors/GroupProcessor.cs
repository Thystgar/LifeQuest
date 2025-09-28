using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Models.Storage;
using LifeQuest.Api.Storage;

namespace LifeQuest.Api.Processors
{
    public class GroupProcessor : IGroupProcessor
    {
        private readonly IGroupStorage _groupStorage;
        
        public GroupProcessor(IGroupStorage groupStorage)
        {
            _groupStorage = groupStorage;
        }

        public async Task<GroupApiModel> GetGroupByIdAsync(string id)
        {
            var group = await _groupStorage.GetGroupByIdAsync(id) ?? throw new NullReferenceException("Group not returned");
            return group.ToApiModel();
        }

        public async Task<string> AddGroupAsync(NewGroupApiModel group)
        {
            var storageGroup = new GroupStorageModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = group.Name, 
                Description = group.Description,
                InviteCode = Guid.NewGuid().ToString().Substring(0, 16)
            };
            await _groupStorage.AddGroupAsync(storageGroup);
            return storageGroup.Id;
        }

        public async Task UpdateGroupAsync(GroupApiModel group)
        {
            var groupStorage = group.ToStorageModel();
            await _groupStorage.UpdateGroupAsync(groupStorage);
        }

        public async Task<GroupApiModel> GetGroupByInviteCodeAsync(string inviteCode)
        {
            var group = await _groupStorage.GetGroupByInviteCodeAsync(inviteCode) ?? throw new NullReferenceException("Group not returned");
            return group.ToApiModel();
        }
    }
}
