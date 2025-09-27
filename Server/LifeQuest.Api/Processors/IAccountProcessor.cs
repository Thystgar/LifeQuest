using LifeQuest.Api.Models.API;

namespace LifeQuest.Api.Processors
{
    public interface IAccountProcessor
    {
        Task<AccountApiModel?> GetAccountOrCreateAsync();
        Task<AccountApiModel?> GetMyAccountAsync();
        Task AddPointsAsync(string userId, int points);
        Task SpendPointsAsync(string userId, int points);
        Task JoinGroupAsync(string userId, string inviteCode);
    }
}
