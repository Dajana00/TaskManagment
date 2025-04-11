using Microsoft.EntityFrameworkCore;
using Trello.Data;
using Trello.Model;
using Trello.Repository.IRepository;

namespace Trello.Repository
{
    public class SprintRepository : ISprintRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SprintRepository> _logger;

        public SprintRepository(AppDbContext context, ILogger<SprintRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Sprint?> GetActiveByProjectId(int projectId)
        {
            try
            {
                var project = await _context.Sprints
                    .FirstOrDefaultAsync(p => p.ProjectId == projectId);

                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching sprint with project id {projectId}");
                throw new Exception($"An error occurred while retrieving project with this project id: {projectId}. Please try again.");
            }
        }
    }
}
