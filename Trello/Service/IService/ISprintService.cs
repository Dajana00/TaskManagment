using FluentResults;
using Trello.DTOs;
using Trello.Model;

namespace Trello.Service.IService
{
    public interface ISprintService
    {
        Task<Result<SprintDto>> CreateAsync(SprintDto sprintdto);
        Task<Result<SprintDto>> Activate(int id);
        Task<Sprint> GetById(int id);
        Task<Result<ICollection<SprintDto>>> GetByProjectId(int id);
        Task<Result<SprintDto>> GetActiveByProjectId(int id);

    }
}
