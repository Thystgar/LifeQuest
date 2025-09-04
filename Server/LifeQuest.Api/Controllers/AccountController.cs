using LifeQuest.Api.Models.API;
using LifeQuest.Api.Processors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace LifeQuest.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountProcessor _accountProcessor; 

        public AccountController(IAccountProcessor accountProcessor) 
        {
            _accountProcessor = accountProcessor;
        }

        [HttpGet]
        public async Task<ActionResult<AccountApiModel>> GetAccountAsync() 
        {
            // http://schemas.microsoft.com/identity/claims/objectidentifier - id
            // preferred_username - email
            // name - display name
            // http://schemas.microsoft.com/identity/claims/scope - api scope (we only have user now)
            var userId = User.FindFirst(ClaimConstants.ObjectId)?.Value;

            if (userId == null)
            {
                return BadRequest("Username null");
            }

            var account = await _accountProcessor.GetAccountAsync(userId);
            return Ok(account);
        }
    }
}
