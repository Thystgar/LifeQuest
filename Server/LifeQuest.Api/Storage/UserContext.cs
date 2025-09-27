using LifeQuest.Api.Controllers;
using Microsoft.Identity.Web;

namespace LifeQuest.Api.Storage
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<QuestController> _logger;

        public UserContext(IHttpContextAccessor httpContextAccessor, ILogger<QuestController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }


        public string GetUserId()
        {
            if (_httpContextAccessor.HttpContext == null){ 
                throw new NullReferenceException("HTTP context is null.");
            }

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConstants.ObjectId)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation("User ID not found in token");
                throw new NullReferenceException("User ID not found in token.");
            }

            return userId;
        }

        public string GetUserName()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                throw new NullReferenceException("HTTP context is null.");
            }

            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConstants.Name)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                _logger.LogInformation("User Name not found in token");
                throw new NullReferenceException("User Name not found in token.");
            }

            return userName;
        }

        public string GetUserEmail()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                throw new NullReferenceException("HTTP context is null.");
            }

            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConstants.PreferredUserName)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogInformation("User Email not found in token");
                throw new NullReferenceException("User Email not found in token.");
            }

            return userEmail;
        }
    }
}
