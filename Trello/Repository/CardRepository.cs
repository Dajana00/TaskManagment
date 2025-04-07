using Microsoft.EntityFrameworkCore;
using Trello.Data;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;

namespace Trello.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CardRepository> _logger;

        public CardRepository(AppDbContext context, ILogger<CardRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task CreateAsync(Card card)
        {
            try
            {
                _context.Cards.Add(card);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Card added successifully");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while adding card.");
                throw new Exception("A database error occurred while adding the card. Please try again.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Unexpected error occurred while adding card.");
                throw new Exception($"An unexpected error occurred while adding the card. Please try again. {ex}");
            }
        }

        public async Task<ICollection<Card>> GetAll()
        {
            try
            {
                var cards = await _context.Cards.ToListAsync();
                _logger.LogInformation("Successfully fetched all cards.");
                return cards;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all cards.");
                throw new Exception("An error occurred while retrieving all cards. Please try again.");
            }
        }

        public async Task<Card?> GetByIdAsync(int id)
        {
            try
            {
                var card = await _context.Cards
                    .Include(p => p.Comments)
                    .FirstOrDefaultAsync(p => p.Id == id);
                _logger.LogInformation( $"Successiffuly fetching card with id {id}");

                return card;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching card with id {id}");
                throw new Exception($"An error occurred while retrieving card with this id: {id}. Please try again.");
            }
        }

       

        public void Update(Card card)
        {
            try
            {
                _context.Cards.Update(card);
                _logger.LogInformation($"Successiffuly updated card with id {card.Id}");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Error updating card with id {card.Id}");
                throw new Exception($"An error occurred while updating card with this id: {card.Id}. Please try again.");
            }

           
        }



    }
}
