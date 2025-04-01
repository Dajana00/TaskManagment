using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Trello.DTOs;
using Trello.Service.IService;
using Trello.Helpers;
using Trello.Service.Iservice;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Trello.Model;

namespace Trello.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<Result<AuthResponseDto>> RegisterUserAsync(RegisterRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return Result.Fail("Username and password are required.");
            }

            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return Result.Fail(result.Errors.Select(e => e.Description));
                
            }

            var tokens = _jwtService.GenerateTokens(user);
            return Result.Ok(new AuthResponseDto { AccessToken = tokens.accessToken, RefreshToken = tokens.refreshToken });
        }

        public async Task<Result<AuthResponseDto>> LoginAsync(UserLoginDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, request.Password)))
            {
                return Result.Fail("Invalid credentials.");
            }

            var tokens = _jwtService.GenerateTokens(user);
            return Result.Ok(new AuthResponseDto { AccessToken = tokens.accessToken, RefreshToken = tokens.refreshToken });
        }
    }

}
