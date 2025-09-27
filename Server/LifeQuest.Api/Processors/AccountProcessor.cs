using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Storage;

namespace LifeQuest.Api.Processors
{
    public class AccountProcessor : IAccountProcessor
    {
        private readonly IAccountStorage _account;
        public AccountProcessor(IAccountStorage account) 
        {
            _account = account;
        }
        public async Task AddPointsAsync(string userId,int points) 
        {
            var account =await _account.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("User not returned");
            account.Points += points;
            await _account.UpdateAccountAsync(account);
        }

        public async Task SpendPointsAsync(string userId, int points)
        {
            var account = await _account.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("User not returned");
            account.Points -= points;
            await _account.UpdateAccountAsync(account);
        }

        public async Task<AccountApiModel?> GetAccountAsync(string userId)
        {
            var account = await _account.GetAccountByIdAsync(userId);
            return account?.ToApiModel();
        }

        public async Task JoinGroupAsync(string userId, string inviteCode)
        {
            var account = await _account.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("User not returned");
            account.GroupId = inviteCode;
            await _account.UpdateAccountAsync(account);
        }
    }
}
