using FluentResults;
using Trello.DTOs;

namespace Trello.Service.IService
{
    public interface IUserStoryService
    {
        Task<Result<UserStoryDto>> CreateAsync(UserStoryDto userStoryDto);
        Task<Result<ICollection<UserStoryDto>>> GetByBacklogIdAsync(int backlogId);
        Task<Result<ICollection<UserStoryDto>>> GetAll();
        Task<Result<UserStoryDto>> GetByIdAsync(int id);
        Task<Result<UserStoryDto>> Update(int id, UserStoryDto updatedStory);
        Task<Result> Delete(int id);

    }
}
