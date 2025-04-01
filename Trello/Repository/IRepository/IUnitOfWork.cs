namespace Trello.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IProjectRepository Projects { get; }
        IBoardRepository Boards { get; }
        IColumnRepository Columns { get; }
        //ITaskRepository Tasks { get; }

        Task SaveAsync();
    }
}
