using System;
using System.Threading.Tasks;
using Trello.DTOs;
using Trello.Model;
using FluentResults;

namespace Trello.Service.Iservice
{
    public interface IUserService
    {
        Task<Result<UserDto>> CreateUserAsync(RegisterRequestDto request);
        Task<Result<UserDto>> GetByEmailAsync(string email);
        Task<Result<UserDto>> GetUserByIdAsync(int id);
        Task<Result<UserDto>> GetByUsernameAsync(string username);
        Task<Result> SaveAsync(User user);
        Task<Result> UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiry);
    }
}
