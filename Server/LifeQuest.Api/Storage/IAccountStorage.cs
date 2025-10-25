using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Storage
{
    public interface IAccountStorage
    {
        Task<AccountStorageModel?> GetAccountByIdAsync(string id);
        Task UpdateAccountAsync(AccountStorageModel account);
        Task CreateNewAccount(string userId, string userName);
        Task DeleteAccountAsync(string accountId);
    }
}