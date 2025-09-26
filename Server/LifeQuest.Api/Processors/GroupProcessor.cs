using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
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

        public async Task<IEnumerable<GroupApiModel>> GetGroupsAsync()
        {
            var groups = await _groupStorage.GetGroupAsync();
            return groups.Select(g => g.ToApiModel());
        }

        public async Task<GroupApiModel> GetGroupByIdAsync(string id)
        {
            var group = await _groupStorage.GetGroupByIdAsync(id) ?? throw new NullReferenceException("Group not returned");
            return group.ToApiModel();
        }

        public async Task AddGroupAsync(GroupApiModel group)
        {
            var storageGroup = group.ToStorageModel();
            storageGroup.Id = Guid.NewGuid().ToString();
            await _groupStorage.UpdateGroupAsync(storageGroup);
        }

        public async Task UpdateGroupAsync(GroupApiModel group)
        {
            var groupStorage = new Models.Storage.GroupStorageModel
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description
            };
            await _groupStorage.UpdateGroupAsync(groupStorage);
        }
    }
}
