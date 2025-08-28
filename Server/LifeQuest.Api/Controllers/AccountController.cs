using LifeQuest.Api.Models.API;
using LifeQuest.Api.Processors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var userId = User.FindFirst("preferred_username")?.Value;

            if (userId == null)
            {
                return BadRequest("Username null");
            }

            var account = await _accountProcessor.GetAccountAsync(userId);
            return Ok(account);
        }
    }
}
