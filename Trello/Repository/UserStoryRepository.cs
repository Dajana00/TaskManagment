using Microsoft.EntityFrameworkCore;
using Trello.Data;
using Trello.Model;
using Trello.Repository.IRepository;

namespace Trello.Repository
{
    public class UserStoryRepository : IUserStoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserStoryRepository> _logger;

        public UserStoryRepository(AppDbContext context, ILogger<UserStoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task CreateAsync(UserStory userStory)
        {
            try
            {
                _context.UserStories.Add(userStory);
                await _context.SaveChangesAsync();
                _logger.LogInformation("UserStory added successifully");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while adding userStory.");
                throw new Exception("A database error occurred while adding the userStory. Please try again.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Unexpected error occurred while adding userStory.");
                throw new Exception($"An unexpected error occurred while adding the userStory. Please try again. {ex}");
            }
        }

    }
}
