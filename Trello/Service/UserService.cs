using System;
using System.Threading.Tasks;
using Trello.DTOs;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Helpers;
using Trello.Service.Iservice;
using FluentResults;

namespace Trello.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserDto>> CreateUserAsync(RegisterRequestDto request)
        {
            if (request == null)
                return Result.Fail("Registration request cannot be null.");

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Username))
                return Result.Fail("Email and username are required.");

            if (await _unitOfWork.Users.UserExistsAsync(request.Email, request.Username))
                return Result.Fail("User with this email or username already exists.");

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Username,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = PasswordHasher.HashPassword(request.Password)
            };

            await _unitOfWork.Users.AddUserAsync(user);

            return Result.Ok(MapToUserDto(user));
        }

        public async Task<Result<UserDto>> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result.Fail("Email cannot be empty.");

            var user = await _unitOfWork.Users.GetUserByEmailAsync(email);
            if (user == null)
                return Result.Fail("User with this email was not found.");

            return Result.Ok(MapToUserDto(user));
        }

        public async Task<Result<UserDto>> GetUserByIdAsync(int id)
        {
            if (id <= 0)
                return Result.Fail("Invalid user ID. ID must be greater than zero.");

            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (user == null)
                return Result.Fail("User with this ID was not found.");

            return Result.Ok(MapToUserDto(user));
        }
        public async Task<User> GetByIdAsync(int id)
        {
            if (id <= 0)
               throw new Exception("Invalid user ID. ID must be greater than zero.");

            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            return user;
        }

        public async Task<Result<UserDto>> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return Result.Fail("Username cannot be empty."); 

            var user = await _unitOfWork.Users.GetUserByUsernameAsync(username);
            if (user == null)
                return Result.Fail("User with this username was not found.");

            return Result.Ok(MapToUserDto(user));
        }

        public async Task<Result> SaveAsync(User user)
        {
            if (user == null)
                return Result.Fail("User cannot be null.");

            await _unitOfWork.Users.SaveUserAsync(user);
            return Result.Ok();
        }

        public async Task<Result> UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiry)
        {
            var userResult = await GetUserByIdAsync(userId);
            if (userResult.IsFailed)
                return Result.Fail(userResult.Errors);

            var user = userResult.Value;

            //user.RefreshToken = refreshToken;
            //user.RefreshTokenExpiry = expiry;

           // await _unitOfWork.Users.SaveUserAsync(user);
            return Result.Ok();
        }

        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<Result<ICollection<UserDto>>> GetByProjectIdAsync(int projectId)
        {
            if (projectId <= 0)
                return Result.Fail("Invalid project ID. ID must be greater than zero.");

            try
            {
                var users = await _unitOfWork.Users.GetByProjectIdAsync(projectId);
                if (users == null || !users.Any())
                    return Result.Ok((ICollection<UserDto>)new List<UserDto>());

                var userDtos = users.Select(MapToUserDto).ToList();
                return Result.Ok((ICollection<UserDto>)userDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while fetching users for project ID {projectId}: {ex.Message}");
            }
        }

        public async Task<Result<ICollection<UserDto>>> GetNonMembers(int projectId)
        {
            if (projectId <= 0)
                return Result.Fail("Invalid project ID. ID must be greater than zero.");

            try
            {
                var users = await _unitOfWork.Users.GetNonMembers(projectId);

                if (users == null || !users.Any())
                    return Result.Fail("All users are project members.");

                var userDtos = users.Select(MapToUserDto).ToList();

                return Result.Ok((ICollection<UserDto>)userDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while fetching users not members for project ID {projectId}: {ex.Message}");
            }
        }
        public async Task<Result> AddUserToProjectAsync(int projectId, int userId)
        {
            if (userId <= 0 || projectId <= 0)
                return Result.Fail("User ID and Project ID must be greater than zero.");

            try
            {
                await _unitOfWork.Users.AddUserToProjectAsync(projectId, userId);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while adding user {userId} to project {projectId}: {ex.Message}");
            }
        }


    }
}
