using FluentResults;
using Trello.DTOs;
using Trello.Model;

namespace Trello.Service.IService
{
    public interface IProjectService
    {
        Task<Result<ProjectDto>> CreateAsync(ProjectDto project);
        Task<Result<ICollection<ProjectDetailsDto>>> GetUserProjects(int userId);
        Task<Result<ProjectDetailsDto>> GetById(int id);
        Task<Result<ProjectDetailsDto>> GetByUserStory(int userStoryId);

    }
}
