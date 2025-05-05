using Microsoft.AspNetCore.Mvc;
using Trello.DTOs;
using Trello.Service.IService;
using System;
using System.Threading.Tasks;
using Trello.Service.Iservice;
using FluentResults;
using Trello.Filters;

namespace Trello.Controller
{
    [Route("api/auth")]
    [ResultFilter]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequestDto userDto)
        {
            var response = await _authService.RegisterUserAsync(userDto);
            return Ok(response);
          
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var response = await _authService.LoginAsync(userDto);
            return Ok(response);
            
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            return Ok(result.Value);
        }

    }
}
