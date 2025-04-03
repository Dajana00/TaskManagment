using Microsoft.AspNetCore.Mvc;
using Trello.DTOs;
using Trello.Service;
using Trello.Service.IService;

namespace Trello.Controller
{
    [Route("api/userStory")]
    [ApiController]
    public class UserStoryController : ControllerBase
    {
        private readonly IUserStoryService _userStoryService;

        public UserStoryController(IUserStoryService userStoryService)
        {
            _userStoryService = userStoryService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserStoryDto userStoryDto)
        {
            var response = await _userStoryService.CreateAsync(userStoryDto);
            return Ok(response);

        }
    }
}
