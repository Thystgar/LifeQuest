using LifeQuest.Api.Models.API;
using LifeQuest.Api.Processors;
using LifeQuest.Api.Storage;
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
        private readonly IUserContext _userContext;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountProcessor accountProcessor, IUserContext userContext, ILogger<AccountController> logger) 
        {
            _accountProcessor = accountProcessor;
            _userContext = userContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<AccountApiModel>> GetAccountAsync() 
        {
            var account = await _accountProcessor.GetAccountOrCreateAsync();
            return Ok(account);
        }

        [HttpPut]
        public async Task<ActionResult> JoinGroupAsync([FromBody] string inviteCode)
        {
            if (string.IsNullOrWhiteSpace(inviteCode))
            {
                return BadRequest("Group ID cannot be empty.");
            }
            var userId = _userContext.GetUserId();
            _logger.LogInformation("User {UserId} joining group {GroupId}", userId, inviteCode);
            await _accountProcessor.JoinGroupAsync(userId, inviteCode);
            return NoContent();
        }
    }
}
