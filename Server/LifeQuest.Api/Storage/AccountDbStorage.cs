using LifeQuest.Api.Models.Storage;
using Microsoft.EntityFrameworkCore;

namespace LifeQuest.Api.Storage
{
    public class AccountDbStorage : IAccountStorage
    {
        private readonly LifeQuestContext _context;

        public AccountDbStorage(LifeQuestContext context)
        {
            _context = context;
        }

        public async Task<AccountStorageModel?> GetAccountByIdAsync(string id)
        {
            return await _context.Accounts.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAccountAsync(AccountStorageModel account)
        {
            await _context.Accounts
                .Where(a => a.Id == account.Id)
                .ExecuteUpdateAsync(a => a
                    .SetProperty(a => a.Name, account.Name)
                    .SetProperty(a => a.Points, account.Points)
                    .SetProperty(a => a.GroupId, account.GroupId));
        }

        public async Task CreateNewAccount(string userId, string userName)
        {
            var newAccount = new AccountStorageModel
            {
                Id = userId,
                Name = userName,
                Points = 0,
                GroupId = ""
            };
            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(string accountId)
        {
            await _context.Accounts.Where(a => a.Id == accountId).ExecuteDeleteAsync();
        }
    }
}