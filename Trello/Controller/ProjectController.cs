using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.DTOs;
using Trello.ExeptionHandlingResultFilter;

using Trello.Service.IService;

namespace Trello.Controller
{
    [ResultFilter]
    [Route("api/projects")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ProjectDto projectDto)
        {
            var response = await _projectService.CreateAsync(projectDto);
            return Ok(response);

        }

        [HttpGet("getUserProjects/{id}")]
        public async Task<IActionResult> GetUserProjects([FromRoute] int id)
        {
            var response = await _projectService.GetUserProjects(id);
            return Ok(response.Value);

        }
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _projectService.GetById(id);
            return Ok(response.Value);

        }

       
    }


}

