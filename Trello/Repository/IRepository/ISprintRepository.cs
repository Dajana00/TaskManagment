using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface ISprintRepository
    {
        Task<Sprint?> GetActiveByProjectId(int projectId);
        Task<Sprint> GetById(int id);
        Task<Sprint> Activate(int id);
        Task<ICollection<Sprint>> GetByProjectId(int projectId);
        Task CreateAsync(Sprint sprint);
    }
}
