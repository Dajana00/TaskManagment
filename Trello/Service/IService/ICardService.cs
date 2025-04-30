using FluentResults;
using Trello.DTOs;
using Trello.Model;

namespace Trello.Service.IService
{
    public interface ICardService
    {
        Task<Result<CardDto>> CreateAsync(CreateCardDto card);
        Task<Result<CardDto>> GetByIdAsync(int id);
        Task<Result> UpdateCardStatus(MoveCardDto moveCarddto);
       // Task<Result> AddToActiveSprint(int id);
        Task<Result<ICollection<CardDto>>> GetByBoardId(int boardId);
        Task<Result<ICollection<CardDto>>> GetByUserStoryId(int id);
        Task<bool> AreAllCardsDone(int boardId);
        Task<Result> UpdateAsync(Card card);


    }
}
