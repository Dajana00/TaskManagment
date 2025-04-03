using Microsoft.AspNetCore.Mvc;
using Trello.Service.IService;

namespace Trello.Controller
{
    [Route("api/backlog")]
    [ApiController]
    public class BacklogController : ControllerBase
    {
        private readonly IBacklogService _backlogService;

        public BacklogController(IBacklogService backlogService)
        {
            _backlogService = backlogService;
        }


        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _backlogService.GetById(id);
            return Ok(response.Value);

        }
    }
}
