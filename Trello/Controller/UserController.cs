using Microsoft.AspNetCore.Mvc;
using Trello.DTOs;
using Trello.Model;
using System.Threading.Tasks;
using Trello.Service.Iservice;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Trello.Service;
using Trello.Service.IService;

namespace Trello.Controller
{
    
        [ApiController]
        [Route("api/user")]
        [Authorize]
        public class UserController : BaseApiController
        {
            private readonly IAuthService _authService;
            private readonly IUserService _userService;

        public UserController(IAuthService authService,IUserService userService)
        {
             _authService = authService;
             _userService = userService;

        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            return Ok(response.Value);    
        }

        [HttpGet("getByProjectId/{projectId}")]
        public async Task<IActionResult> GetByProjectId([FromRoute]int projectId)
        {
            var response = await _userService.GetByProjectIdAsync(projectId);
            return Ok(response.Value);
        }

        [HttpGet("getNonMembers/{projectId}")]
        public async Task<IActionResult> GetNonMembers([FromRoute] int projectId)
        {
            var response = await _userService.GetNonMembers(projectId);
            return Ok(response.Value);
        }

        [HttpPost("addOnProject/{projectId}/{userId}")]
        public async Task<IActionResult> AddUserOnProject( int projectId,int userId)
        {
            var response = await _userService.AddUserToProjectAsync(projectId, userId);
            return Ok(response);
        }
    }
}

