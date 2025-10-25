using LifeQuest.Api.Models.API;

namespace LifeQuest.Api.Processors
{
    public interface IAccountProcessor
    {
        Task<AccountApiModel?> GetAccountOrCreateAsync();
        Task<AccountApiModel?> GetMyAccountAsync();
        Task AddPointsAsync(int points);
        Task SpendPointsAsync(int points);
        Task JoinGroupAsync(string inviteCode);
        Task LeaveGroupAsync();
        Task DeleteAccountAsync();
    }
}
