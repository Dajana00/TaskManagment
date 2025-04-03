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
        public ICardRepository Cards { get; private set; }
        public IBacklogRepository Backlogs { get; private set; }
        public IUserStoryRepository UserStories { get; private set; }

        private readonly ILoggerFactory _loggerFactory;  


        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _loggerFactory = loggerFactory;
            Users = new UserRepository(_context, _loggerFactory.CreateLogger<UserRepository>());
            Projects = new ProjectRepository(_context, _loggerFactory.CreateLogger<ProjectRepository>());
            Boards = new BoardRepository(_context, _loggerFactory.CreateLogger<BoardRepository>());
            Cards = new CardRepository(_context, _loggerFactory.CreateLogger<CardRepository>());
            Backlogs = new BacklogRepository(_context, _loggerFactory.CreateLogger<BacklogRepository>());
            UserStories = new UserStoryRepository(_context, _loggerFactory.CreateLogger<UserStoryRepository>());
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
