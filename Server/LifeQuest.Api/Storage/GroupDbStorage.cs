using LifeQuest.Api.Models.Storage;
using Microsoft.EntityFrameworkCore;

namespace LifeQuest.Api.Storage
{
    public class GroupDbStorage : IGroupStorage
    {
        private readonly LifeQuestContext _context;
        public GroupDbStorage(LifeQuestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupStorageModel>> GetGroupsAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<GroupStorageModel?> GetGroupByIdAsync(string id)
        {
            return await _context.Groups.SingleOrDefaultAsync(g => g.Id == id);
        }

        public async Task UpdateGroupAsync(GroupStorageModel group)
        {
            await _context.Groups
                .Where(g => g.Id == group.Id)
                .ExecuteUpdateAsync(g => g
                    .SetProperty(g => g.Name, group.Name)
                    .SetProperty(g => g.Description, group.Description)
                    .SetProperty(g => g.InviteCode, group.InviteCode));
        }

        public async Task<GroupStorageModel?> GetGroupByInviteCodeAsync(string inviteCode)
        {
            return await _context.Groups.SingleOrDefaultAsync(g => g.InviteCode == inviteCode);
        }

        public async Task AddGroupAsync(GroupStorageModel group)
        {
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
        }
    }
}
