using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Storage;

namespace LifeQuest.Api.Processors
{
    public class AccountProcessor : IAccountProcessor
    {
        private readonly IAccountStorage _storage;
        public AccountProcessor(IAccountStorage storage) 
        {
            _storage = storage;
        }
        public async Task AddPointsAsync(string userId,int points) 
        {
            var user =await _storage.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("User not returned");
            user.Points += points;
            await _storage.UpdateAccountAsync(user);
        }

        public async Task SpendPointsAsync(string userId, int points)
        {
            var user = await _storage.GetAccountByIdAsync(userId) ?? throw new NullReferenceException("User not returned");
            user.Points -= points;
            await _storage.UpdateAccountAsync(user);
        }

        public async Task<AccountApiModel?> GetAccountAsync(string userId)
        {
            var user = await _storage.GetAccountByIdAsync(userId);
            return user?.ToApiModel();
        }
    }
}
