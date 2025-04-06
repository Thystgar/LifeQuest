using LifeQuest.Api.Models.Storage;
using LifeQuest.Api.Storage;

namespace LifeQuest.Api.Processors
{
    public class AccountProcessor
    {
        private readonly ILifeQuestStorage _storage;
        public AccountProcessor(ILifeQuestStorage storage) 
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
