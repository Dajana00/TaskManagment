using Microsoft.EntityFrameworkCore;
using Trello.Data;
using Trello.Model;
using Trello.Repository.IRepository;

namespace Trello.Repository
{
    public class ProjectRepository: IProjectRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProjectRepository> _logger;

        public ProjectRepository(AppDbContext context, ILogger<ProjectRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task CreateAsync(Project project)
        {
            try
            {
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Project added successifully");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while adding project.");
                throw new Exception("A database error occurred while adding the project. Please try again.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Unexpected error occurred while adding project.");
                throw new Exception($"An unexpected error occurred while adding the project. Please try again. {ex}");
            }
        }

        public async Task<ICollection<Project>> GetUserProjects(int userId)
        {
            try
            {
                var projects = await _context.Projects
                    .Where(p => p.UserProjects.Any(up => up.UserId == userId) || p.OwnerId == userId)
                    .ToListAsync();

                return projects;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching projects for user {userId}", userId);
                throw new Exception("An error occurred while retrieving user projects. Please try again.");
            }
        }
        public async Task<Project> GetById(int id)
        {
            try
            {
                var project = await _context.Projects
                    .Include(p => p.Owner)
                    .Include(p => p.UserProjects).ThenInclude(up => up.User)
                    .Include(p => p.Sprints)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching project with id {id}");
                throw new Exception($"An error occurred while retrieving project with this id: {id}. Please try again.");
            }
        }

    }
}
