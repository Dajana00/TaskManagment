using Microsoft.EntityFrameworkCore;
using Trello.Data;
using Trello.Model;
using Trello.Repository.IRepository;

namespace Trello.Repository
{
    public class BacklogRepository : IBacklogRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BacklogRepository> _logger;

        public BacklogRepository(AppDbContext context, ILogger<BacklogRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task CreateAsync(Backlog backlog)
        {
            try
            {
                _context.Backlogs.Add(backlog);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Backlog added successifully");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while adding backlog.");
                throw new Exception("A database error occurred while adding the backlog. Please try again.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Unexpected error occurred while adding backlog.");
                throw new Exception($"An unexpected error occurred while adding the backlog. Please try again. {ex}");
            }
        }

        public async Task<Backlog> GetById(int id)
        {
            try
            {
                var backlog = await _context.Backlogs
                    .FirstOrDefaultAsync(p => p.Id == id);

                return backlog;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching backlog with id {id}");
                throw new Exception($"An error occurred while retrieving backlog with this id: {id}. Please try again.");
            }
        }
    }
}
