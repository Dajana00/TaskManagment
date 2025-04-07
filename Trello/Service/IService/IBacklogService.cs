using FluentResults;
using Trello.DTOs;
using Trello.Model;

namespace Trello.Service.IService
{
    public interface IBacklogService
    {
        Task<Result<Backlog>> CreateDefaultBacklogAsync(Project project);
        Task<Result<BacklogDto>> GetById(int id);
    }
}
