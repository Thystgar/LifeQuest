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

            var UserId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConstants.ObjectId)?.Value;

            if (string.IsNullOrEmpty(UserId))
            {
                _logger.LogInformation("User ID not found in token");
                throw new NullReferenceException("User ID not found in token.");
            }

            return UserId;
        }
    }
}
