using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface ISprintRepository
    {
        Task<Sprint?> GetActiveByProjectId(int projectId);
    }
}
