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
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(new { Errors = response.Errors.Select(e => e.Reasons) });

            }


        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
           
            var response = await _authService.LoginAsync(userDto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(new { Errors = response.Errors.Select(e => e.Reasons) });
        }
    }
}
