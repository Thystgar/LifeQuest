using LifeQuest.Api.Models.Storage;
using LifeQuest.Api.Storage;

namespace LifeQuest.Api.Processors
{
    public class AccountProcessor
    {
        private readonly IAccountStorage _storage;
        public AccountProcessor(IAccountStorage storage) 
        {
            _storage = storage;
        }
        public async Task AddPointsAsync(string userId,int points) 
        {
            var user =await _storage.GetAccountByIdAsync(userId);

            if (user == null) { throw new NullReferenceException("User not returned"); }

            user.Points += points;

            await _storage.UpdateAccountAsync(user);
        }
    }
}
