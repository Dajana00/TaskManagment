using Trello.Data;
using Trello.Repository.IRepository;

namespace Trello.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository Users { get; private set; }
        public IProjectRepository Projects { get; private set; }
        public IBoardRepository Boards { get; private set; }
        public IColumnRepository Columns { get; private set; }
        private readonly ILoggerFactory _loggerFactory;  


        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _loggerFactory = loggerFactory;
            Users = new UserRepository(_context, _loggerFactory.CreateLogger<UserRepository>());
            Projects = new ProjectRepository(_context, _loggerFactory.CreateLogger<ProjectRepository>());
            Boards = new BoardRepository(_context, _loggerFactory.CreateLogger<BoardRepository>());
            Columns = new ColumnRepository(_context, _loggerFactory.CreateLogger<ColumnRepository>());
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
