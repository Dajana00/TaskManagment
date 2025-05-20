using Microsoft.AspNetCore.Mvc;
using Trello.DTOs;
using Trello.ExeptionHandlingResultFilter;
using Trello.Service;
using Trello.Service.IService;

namespace Trello.Controller
{
    [ResultFilter]
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

        [HttpGet("getByBacklogId/{backlogId}")]
        public async Task<IActionResult> GetByBacklogId(int backlogId)
        {
            var response = await _userStoryService.GetByBacklogIdAsync(backlogId);
            return Ok(response.Value);

        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _userStoryService.GetAll();
            return Ok(response);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetAll([FromRoute] int id)
        {
            var response = await _userStoryService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserStoryDto updatedStory)
        {
            var result = await _userStoryService.Update(id, updatedStory);
            return Ok(result.Value);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userStoryService.Delete(id);
            return NoContent();
        }

    }
}
