using Microsoft.EntityFrameworkCore;
using Trello.Data;
using Trello.Model;
using Trello.Repository.IRepository;

namespace Trello.Repository
{
    public class BoardRepository : IBoardRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BoardRepository> _logger;

        public BoardRepository(AppDbContext context, ILogger<BoardRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task CreateAsync(Board board)
        {
            try
            {
                _context.Boards.Add(board);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Boards added successifully");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while adding board.");
                throw new Exception("A database error occurred while adding the boards. Please try again.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Unexpected error occurred while adding board.");
                throw new Exception($"An unexpected error occurred while adding the board. Please try again. {ex}");
            }
        }

        public async Task<Board> GetById(int id)
        {
            try
            {
                var board = await _context.Boards
                    .Include(p => p.Columns)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return board;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching board with id {id}");
                throw new Exception($"An error occurred while retrieving board with this id: {id}. Please try again.");
            }
        }

        
    }
}
