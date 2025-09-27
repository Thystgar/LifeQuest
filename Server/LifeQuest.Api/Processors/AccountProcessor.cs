using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Storage;

namespace LifeQuest.Api.Processors
{
    public class AccountProcessor : IAccountProcessor
    {
        private readonly IAccountStorage _account;
        private readonly IUserContext _userContext;

        public AccountProcessor(IAccountStorage account, IUserContext userContext)
        {
            _account = account;
            _userContext = userContext;
        }

        public async Task<AccountApiModel?> GetAccountOrCreateAsync(string userId)
        {
            var account = await _account.GetAccountByIdAsync(userId);
            if (account == null)
            {
                var userName = _userContext.GetUserName() ?? throw new NullReferenceException("UserName not found in context");

                await _account.CreateNewAccount(userId, userName);
                account = await _account.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("Account not returned");
            }
            return account.ToApiModel();
        }

        public async Task<AccountApiModel?> GetMyAccountAsync()
        {
            var userId = _userContext.GetUserId() ?? throw new NullReferenceException("UserId not found in context");
            var account = await _account.GetAccountByIdAsync(userId);
            return account?.ToApiModel();
        }

        public async Task AddPointsAsync(string userId, int points)
        {
            var account = await _account.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("User not returned");
            account.Points += points;
            await _account.UpdateAccountAsync(account);
        }

        public async Task SpendPointsAsync(string userId, int points)
        {
            var account = await _account.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("User not returned");
            account.Points -= points;
            await _account.UpdateAccountAsync(account);
        }

        public async Task JoinGroupAsync(string userId, string inviteCode)
        {
            var account = await _account.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("User not returned");
            account.GroupId = inviteCode;
            await _account.UpdateAccountAsync(account);
        }
    }
}
