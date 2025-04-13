using LifeQuest.Api.Models.API;
using LifeQuest.Api.Processors;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountProcessor _accountProcessor; 

        public AccountController(IAccountProcessor accountProcessor) 
        {
            _accountProcessor = accountProcessor;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<AccountApiModel>> GetAccountAsync(string userId) 
        {
            var account = await _accountProcessor.GetAccountAsync(userId);
            return Ok(account);
        }
    }
}
