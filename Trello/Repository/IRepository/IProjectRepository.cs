using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface IProjectRepository
    {
        Task CreateAsync(Project project);
        Task<ICollection<Project>> GetUserProjects(int userId);
        Task<Project> GetById(int id);

    }
}
