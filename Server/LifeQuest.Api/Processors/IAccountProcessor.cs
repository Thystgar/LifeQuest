using LifeQuest.Api.Models.API;

namespace LifeQuest.Api.Processors
{
    public interface IAccountProcessor
    {
        Task AddPointsAsync(string userId, int points);
        Task SpendPointsAsync(string userId, int points);
        Task<AccountApiModel?> GetAccountAsync(string userId);
        Task JoinGroupAsync(string userId, string inviteCode);
    }
}
