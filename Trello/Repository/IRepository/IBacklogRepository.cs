using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface IBacklogRepository
    {
        Task CreateAsync(Backlog backlog);
        Task<Backlog> GetById(int id);
    }
}
