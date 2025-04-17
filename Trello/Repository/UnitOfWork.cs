using Trello.Data;
using Trello.Repository.IRepository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IUserRepository Users { get; }
    public IProjectRepository Projects { get; }
    public IBoardRepository Boards { get; }
    public ICardRepository Cards { get; }
    public IBacklogRepository Backlogs { get; }
    public IUserStoryRepository UserStories { get; }
    public ISprintRepository Sprints { get; }

    public UnitOfWork(
        AppDbContext context,
        IUserRepository users,
        IProjectRepository projects,
        IBoardRepository boards,
        ICardRepository cards,
        IBacklogRepository backlogs,
        IUserStoryRepository userStories,
        ISprintRepository sprintRepository)
    {
        _context = context;
        Users = users;
        Projects = projects;
        Boards = boards;
        Cards = cards;
        Backlogs = backlogs;
        UserStories = userStories;
        Sprints = sprintRepository;
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
