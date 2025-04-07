using FluentResults;
using Trello.DTOs;

namespace Trello.Service.IService
{
    public interface ICardService
    {
        Task<Result<CardDto>> CreateAsync(CardDto card);
        Task<Result> UpdateCardStatus(MoveCardDto moveCarddto);
        Task<Result<ICollection<CardDto>>> GetAll();

    }
}
