using FluentResults;

namespace Trello.Service.IService
{
    public interface ICardSprintService
    {
        Task<Result> AddCardToActiveSprint(int cardId);

    }
}
