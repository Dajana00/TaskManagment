using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface IUserStoryRepository
    {
        Task CreateAsync(UserStory userStory);
    }
}
