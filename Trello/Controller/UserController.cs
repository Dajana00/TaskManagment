using Microsoft.AspNetCore.Mvc;
using Trello.DTOs;
using Trello.Model;
using System.Threading.Tasks;
using Trello.Service.Iservice;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Trello.Service;

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



    }
}

