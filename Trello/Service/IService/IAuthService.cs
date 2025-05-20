using FluentResults;
using Trello.DTOs;

namespace Trello.Service.Iservice
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> RegisterUserAsync(RegisterRequestDto request);
        Task<Result<AuthResponseDto>> LoginAsync(UserLoginDto request);

        Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken);
    }

}
