namespace Trello.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IProjectRepository Projects { get; }
        IBoardRepository Boards { get; }
        ICardRepository Cards { get; }
        IBacklogRepository Backlogs { get; }
        IUserStoryRepository UserStories { get; }
        //ITaskRepository Tasks { get; }

        Task SaveAsync();
    }
}
