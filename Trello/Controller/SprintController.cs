using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.DTOs;
using Trello.Filters;
using Trello.Service.IService;

namespace Trello.Controller
{
    [ResultFilter]
    [Route("api/sprint")]
    [ApiController]
    [Authorize]
    public class SprintController : ControllerBase
    {
        private readonly ISprintService _sprintService;

        public SprintController(ISprintService sprintService)
        {
            _sprintService = sprintService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SprintDto sprintDto)
        {
            var response = await _sprintService.CreateAsync(sprintDto);
            return Ok(response);

        }

        [HttpGet("getByProjectId/{id}")]
        public async Task<IActionResult> GetByProjectId([FromRoute] int id)
        {
            var response = await _sprintService.GetByProjectId(id);
            return Ok(response.Value);

        }
        [HttpPut("activateSprint/{id}")]
        public async Task<IActionResult> ActivateSprint([FromRoute] int id)
        {
            var response = await _sprintService.Activate(id);
            return Ok(response);

        }
        [HttpPut("complete/{boardId}")]
        public async Task<IActionResult> Complete([FromRoute] int boardId)
        {
            var response = await _sprintService.Complete(boardId);
            return Ok(response);

        }
    }
}
