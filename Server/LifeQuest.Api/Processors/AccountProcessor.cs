using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Storage;

namespace LifeQuest.Api.Processors
{
    public class AccountProcessor : IAccountProcessor
    {
        private readonly IAccountStorage _account;
        private readonly IUserContext _userContext;
        private readonly IGroupStorage _group;

        public AccountProcessor(IAccountStorage account, IUserContext userContext, IGroupStorage group)
        {
            _account = account;
            _userContext = userContext;
            _group = group;
        }

        public async Task<AccountApiModel?> GetAccountOrCreateAsync()
        {
            var userId = _userContext.GetUserId() ?? throw new NullReferenceException("UserId not found in context");
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

        public async Task AddPointsAsync(int points)
        {
            var userId = _userContext.GetUserId() ?? throw new NullReferenceException("UserId not found in context");
            var account = await _account.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("Account not returned");
            account.Points += points;
            await _account.UpdateAccountAsync(account);
        }

        public async Task SpendPointsAsync(int points)
        {
            var userId = _userContext.GetUserId() ?? throw new NullReferenceException("UserId not found in context");
            var account = await _account.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("Account not returned");
            account.Points -= points;
            await _account.UpdateAccountAsync(account);
        }

        public async Task JoinGroupAsync(string inviteCode)
        {
            var userId = _userContext.GetUserId() ?? throw new NullReferenceException("UserId not found in context");
            var account = await _account.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("Account not returned");
            var group = await _group.GetGroupByInviteCodeAsync(inviteCode) ?? throw new NullReferenceException("Group not returned");
            account.GroupId = group.Id;
            await _account.UpdateAccountAsync(account);
        }

        public async Task LeaveGroupAsync()
        {
            // nemame na to metodu GetMyAccount nebo tak neco?
            var userId = _userContext.GetUserId() ?? throw new NullReferenceException("UserId not found in context");
            var account = await _account.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("Account not returned");
            account.GroupId = ""; //string.Empty
            // chceme grupu vymazat kdyz je prazdna?
            await _account.UpdateAccountAsync(account);
        }

        public async Task DeleteAccountAsync()
        {
            // nemame na to metodu GetMyAccount
            var userId = _userContext.GetUserId() ?? throw new NullReferenceException("UserId not found in context");
            var account = await _account.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("Account not returned");
            await _account.DeleteAccountAsync(account.Id);
            // todo vymazat to i v Microsoft entra id
        }

        public async Task UpdateAccountAsync(UpdateAccountApiModel updateAccount)
        {
            var account = await GetMyAccountAsync() ?? throw new NullReferenceException("Account not returned");
            account.TermsAccepted = updateAccount.TermsAccepted;
            await _account.UpdateAccountAsync(account.ToStorageModel());
        }
    }
}
