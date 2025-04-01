using FluentResults;
using Trello.DTOs;
using Trello.Model;

namespace Trello.Service.IService
{
    public interface IBoardService
    {
        Task<Result<Board>> CreateDefaultBoardAsync(Project project);
        Task<Result<BoardDto>> GetById(int id);
    }
}
