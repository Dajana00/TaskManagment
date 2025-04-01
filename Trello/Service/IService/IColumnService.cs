using FluentResults;
using Trello.DTOs;
using Trello.Model;

namespace Trello.Service.IService
{
    public interface IColumnService
    {
        Task<Result<ICollection<Column>>> CreateDefaultBoardColumnsAsync(Board board);
    }
}
