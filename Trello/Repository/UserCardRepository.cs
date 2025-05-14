using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Trello.Data;
using Trello.Model;
using Trello.Repository.IRepository;

namespace Trello.Repository
{
    public class UserCardRepository : IUserCardRepository
    {

        private readonly AppDbContext _context;
        private readonly ILogger<UserCardRepository> _logger;

        public UserCardRepository(AppDbContext context, ILogger<UserCardRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ICollection<User>> GetCardMembersAsync(int cardId)
        {
            try
            {
                _logger.LogInformation("Attempting to fetch users for CardId: {CardId}", cardId);

                var card = await _context.Cards
                    .Include(c => c.UserCards)
                    .ThenInclude(uc => uc.User)
                    .FirstOrDefaultAsync(c => c.Id == cardId);

                if (card == null)
                {
                    _logger.LogWarning("Card with ID {CardId} not found.", cardId);
                    return new List<User>();
                }

                var users = card.UserCards
                    .Select(uc => uc.User)
                    .ToList();

                _logger.LogInformation("Successfully found {UserCount} users for CardId: {CardId}", users.Count, cardId);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users for CardId: {CardId}", cardId);
                throw;
            }
        }


        public async Task<ICollection<User>> GetNonMembers(int cardId, int projectId)
        {
            try
            {
                _logger.LogInformation("Fetching non-member users for CardId: {CardId}", cardId);

                var card = await _context.Cards
                    .Include(c => c.UserCards)
                    .FirstOrDefaultAsync(c => c.Id == cardId);

                if (card == null)
                {
                    _logger.LogWarning("Card with ID {CardId} not found.", cardId);
                    return new List<User>();
                }

                // Uzimamo sve korisnike koji su članovi projekta kojem kartica pripada
                var project = await _context.Projects
                    .Include(p => p.UserProjects)
                    .ThenInclude(up => up.User)
                    .FirstOrDefaultAsync(p => p.Id == projectId);

                if (project == null)
                {
                    _logger.LogWarning("Project with ID {ProjectId} not found for card {CardId}.", cardId);
                    return new List<User>();
                }

                var projectUserIds = project.UserProjects.Select(up => up.UserId).ToHashSet();
                var cardUserIds = card.UserCards.Select(uc => uc.UserId).ToHashSet();

                // Vracamo sve korisnike koji su u projektu ali nisu na kartici
                var nonMembers = project.UserProjects
                    .Where(up => !cardUserIds.Contains(up.UserId))
                    .Select(up => up.User)
                    .ToList();

                _logger.LogInformation("Found {Count} non-members for card {CardId}.");
                return nonMembers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching non-members for CardId: {CardId}", cardId);
                throw;
            }
        }



        public async Task AddUserToCardAsync(int cardId, int userId)
        {
            try
            {
                var card = await _context.Cards
                    .Include(p => p.UserCards)
                    .FirstOrDefaultAsync(p => p.Id == cardId);

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (card == null)
                {
                    _logger.LogError($"Cannot add user to card. Card with ID {cardId} not found.");
                    throw new Exception($"Card with ID {cardId} not found.");
                }

                if (user == null)
                {
                    _logger.LogError($"Cannot add user to card. User with ID {userId} not found.");
                    throw new Exception($"User with ID {userId} not found.");
                }

                bool alreadyExists = card.UserCards.Any(up => up.UserId == userId);
                if (alreadyExists)
                {
                    _logger.LogWarning($" User with ID {userId} is already a member of card {cardId}.");
                    throw new Exception($" User with ID {userId} is already a member of card {cardId}");
                }

                card.UserCards.Add(new UserCard
                {
                    UserId = userId,
                    CardId = cardId,
                    User = user,
                    Card = card
                });

                await _context.SaveChangesAsync();

                _logger.LogInformation($"User {userId} successfully added to card {cardId}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Exception] An error occurred while adding user {userId} to project {cardId}: {ex.Message}");
                throw;
            }
        }

    }
}
