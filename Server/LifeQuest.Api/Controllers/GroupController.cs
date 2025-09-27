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

        public GroupController(IGroupProcessor groupProcessor, ILogger<GroupController> logger, IUserContext userContext)
        {
            _groupProcessor = groupProcessor;
            _logger = logger;
            _userContext = userContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupApiModel>>> GetGroupsAsync()
        {
            var userId = _userContext.GetUserId();
            _logger.LogInformation("Getting all groups for user {UserId}", userId);
            var groups = await _groupProcessor.GetGroupsAsync();
            return Ok(groups);
        }

        [HttpPost]
        public async Task<ActionResult<GroupApiModel>> AddGroupAsync([FromBody] GroupApiModel group)
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
            return NoContent();
        }
    }
}
