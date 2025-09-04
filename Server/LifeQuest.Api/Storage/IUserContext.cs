namespace LifeQuest.Api.Storage
{
    public interface IUserContext
    {
        Task<string> GetUserId();
    }
}
