﻿using System;
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
        Task<Result<ICollection<UserDto>>> GetByProjectIdAsync(int projectId);
        Task<Result<ICollection<UserDto>>> GetNonMembers(int projectId);
        Task<Result> SaveAsync(User user);
        Task<Result> UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiry);
        Task<Result> AddUserToProjectAsync(int projectId, int userId);
    }
}
