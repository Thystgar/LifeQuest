using LifeQuest.Api.Models.API;
using LifeQuest.Api.Processors;
using LifeQuest.Api.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuest.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupProcessor _groupProcessor;
        private readonly ILogger<GroupController> _logger;
        private readonly IUserContext _userContext;
        private readonly IAccountProcessor _accountProcessor;

        public GroupController(IGroupProcessor groupProcessor, ILogger<GroupController> logger, IUserContext userContext, IAccountProcessor accountProcessor)
        {
            _groupProcessor = groupProcessor;
            _logger = logger;
            _userContext = userContext;
            _accountProcessor = accountProcessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupApiModel>>> GetGroupAsync()
        {
            var userId = _userContext.GetUserId();
            _logger.LogInformation("Getting all groups for user {UserId}", userId);
            var group = await _groupProcessor.GetGroupByIdAsync(userId);
            return Ok(group);
        }

        [HttpPost]
        public async Task<ActionResult<GroupApiModel>> AddGroupAsync([FromBody] NewGroupApiModel group)
        {
            if (group == null)
            {
                return BadRequest("Group cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(group.Name))
            {
                return BadRequest("Group name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(group.Description))
            {
                return BadRequest("Group description cannot be empty.");
            }
            
            await _groupProcessor.AddGroupAsync(group);

            var createdGroup = await _groupProcessor.GetGroupByIdAsync(group.Name);
            var userId = _userContext.GetUserId() ?? throw new NullReferenceException("UserId not found in context");
            var inviteCode = createdGroup.InviteCode ?? throw new NullReferenceException("Invite code not found");
            await _accountProcessor.JoinGroupAsync(inviteCode);

            return Ok(group);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateGroupAsync([FromBody] GroupApiModel group)
        {
            if (group == null)
            {
                return BadRequest("Group cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(group.Id))
            {
                return BadRequest("Group ID cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(group.Name))
            {
                return BadRequest("Group name cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(group.Description))
            {
                return BadRequest("Group description cannot be empty.");
            }
            await _groupProcessor.UpdateGroupAsync(group);
            var newGroup = await _groupProcessor.GetGroupByIdAsync(group.Id);
            return Ok(newGroup);
        }
    }
}
