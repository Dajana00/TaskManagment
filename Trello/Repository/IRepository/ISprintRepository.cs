using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface ISprintRepository
    {
        Task<Sprint?> GetActiveByProjectId(int projectId);
        Task<ICollection<Sprint>> GetByProjectId(int projectId);
        Task CreateAsync(Sprint sprint);
    }
}
