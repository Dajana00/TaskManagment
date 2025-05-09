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
                throw new Exception("An error occurred while retrieving the user. Please try again later.",ex);
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
                throw new Exception("An error occurred while retrieving the user. Please try again later.", ex);
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
                throw new Exception("An error occurred while retrieving the user. Please try again later.",ex);
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
                throw new Exception("A database error occurred while adding the user. Please try again.", ex);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Unexpected error occurred while adding user.");
                throw new Exception($"An unexpected error occurred while adding the user. Please try again." ,ex);
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
                throw new Exception("An error occurred while checking if the user exists. Please try again later.", ex  );
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
                throw new Exception("A database error occurred while saving the user. Please try again.",ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while saving user.");
                throw new Exception("An unexpected error occurred while saving the user. Please try again.",ex);
            }
        }

        public async Task<ICollection<User>> GetByProjectIdAsync(int projectId)
        {
            try
            {
                _logger.LogInformation("Attempting to fetch users for ProjectId: {ProjectId}", projectId);

                var project = await _context.Projects
                    .Include(p => p.UserProjects)
                    .ThenInclude(up => up.User)
                    .FirstOrDefaultAsync(p => p.Id == projectId);

                if (project == null)
                {
                    _logger.LogWarning("Project with ID {ProjectId} not found.", projectId);
                    return new List<User>();
                }

                var users = project.UserProjects
                    .Select(up => up.User)
                    .ToList();

                _logger.LogInformation("Successfully found {UserCount} users for ProjectId: {ProjectId}", users.Count, projectId);
                if (users == null)
                {
                    return new List<User>();
                }
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users for ProjectId: {ProjectId}", projectId);
                throw;
            }
        }

        public async Task<ICollection<User>> GetNonMembers(int projectId)
        {
            try
            {
                _logger.LogInformation("Attempting to fetch non-member users for ProjectId: {ProjectId}", projectId);

                var project = await _context.Projects
                    .Include(p => p.UserProjects)
                    .ThenInclude(up => up.User)
                    .FirstOrDefaultAsync(p => p.Id == projectId);

                if (project == null)
                {
                    _logger.LogWarning("Project with ID {ProjectId} not found.", projectId);
                    return new List<User>();
                }

                var allUsers = await _context.Users.ToListAsync();

                var memberIds = project.UserProjects.Select(up => up.UserId).ToHashSet();

                var nonMembers = allUsers.Where(u => !memberIds.Contains(u.Id)).ToList();

                _logger.LogInformation("Successfully found {UserCount} non-members for ProjectId: {ProjectId}", nonMembers.Count, projectId);

                if(nonMembers== null)
                {
                    return new List<User>();
                }
                return nonMembers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching non-member users for ProjectId: {ProjectId}", projectId);
                throw;
            }
        }

        public async Task AddUserToProjectAsync(int projectId, int userId)
        {
            try
            {
                var project = await _context.Projects
                    .Include(p => p.UserProjects)
                    .FirstOrDefaultAsync(p => p.Id == projectId);

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (project == null)
                {
                    _logger.LogError($"Cannot add user to project. Project with ID {projectId} not found.");
                    throw new Exception($"Project with ID {projectId} not found.");
                }

                if (user == null)
                {
                    _logger.LogError($"Cannot add user to project. User with ID {userId} not found.");
                    throw new Exception($"User with ID {userId} not found.");
                }

                bool alreadyExists = project.UserProjects.Any(up => up.UserId == userId);
                if (alreadyExists)
                {
                    _logger.LogWarning($" User with ID {userId} is already a member of project {projectId}.");
                    throw new Exception($" User with ID {userId} is already a member of project {projectId}");
                }

                project.UserProjects.Add(new UserProject
                {
                    UserId = userId,
                    ProjectId = projectId,
                    User = user,
                    Project = project
                });

                await _context.SaveChangesAsync();

               _logger.LogInformation($"User {userId} successfully added to project {projectId}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Exception] An error occurred while adding user {userId} to project {projectId}: {ex.Message}");
                throw;
            }
        }




    }
}
