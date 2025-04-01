using Microsoft.EntityFrameworkCore;
using Trello.Data;
using Trello.Model;
using Trello.Repository.IRepository;

namespace Trello.Repository
{
    public class ColumnRepository : IColumnRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ColumnRepository> _logger;

        public ColumnRepository(AppDbContext context, ILogger<ColumnRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task CreateAsync(Column column)
        {
            try
            {
                _context.Columns.Add(column);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Column added successifully");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while adding board.");
                throw new Exception("A database error occurred while adding the column. Please try again.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Unexpected error occurred while adding column.");
                throw new Exception($"An unexpected error occurred while adding the column. Please try again. {ex}");
            }
        }

        


    }
}
