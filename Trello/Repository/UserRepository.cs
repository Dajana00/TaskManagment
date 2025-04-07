using Microsoft.EntityFrameworkCore;
using Trello.Data;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service;
using Microsoft.Extensions.Logging;
using Trello.Service.IService;

namespace Trello.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user by email.");
                throw new Exception("An error occurred while retrieving the user. Please try again later.");
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user by username.");
                throw new Exception("An error occurred while retrieving the user. Please try again later.");
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user by ID.");
                throw new Exception("An error occurred while retrieving the user. Please try again later.");
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while adding user.");
                throw new Exception("A database error occurred while adding the user. Please try again.");
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Unexpected error occurred while adding user.");
                throw new Exception($"An unexpected error occurred while adding the user. Please try again. {ex}");
            }
        }

        public async Task<bool> UserExistsAsync(string email, string username)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.Email == email || u.UserName == username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if user exists.");
                throw new Exception("An error occurred while checking if the user exists. Please try again later.");
            }
        }

        public async Task SaveUserAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while saving user.");
                throw new Exception("A database error occurred while saving the user. Please try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while saving user.");
                throw new Exception("An unexpected error occurred while saving the user. Please try again.");
            }
        }
    }
}
