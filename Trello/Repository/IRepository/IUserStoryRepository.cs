using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface IUserStoryRepository
    {
        Task CreateAsync(UserStory userStory);
        Task<ICollection<UserStory>> GetByBacklogId(int backlogId);
        Task<ICollection<UserStory>> GetAll();
    }
}
