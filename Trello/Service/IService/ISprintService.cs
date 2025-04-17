using FluentResults;
using Trello.DTOs;

namespace Trello.Service.IService
{
    public interface ISprintService
    {
        Task<Result<SprintDto>> CreateAsync(SprintDto sprintdto);
        Task<Result<ICollection<SprintDto>>> GetByProjectId(int id);

    }
}
