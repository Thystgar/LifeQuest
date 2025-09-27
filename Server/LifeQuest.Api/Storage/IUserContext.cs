namespace LifeQuest.Api.Storage
{
    public interface IUserContext
    {
        string GetUserId();
        string GetUserName();
        string GetUserEmail();
    }
}
