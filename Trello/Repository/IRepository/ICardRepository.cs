using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface ICardRepository
    {
        Task CreateAsync(Card card);
        Task<Card> GetByIdAsync(int cardId);
        Task<ICollection<Card>> GetByActiveSprint(int activeSprintId);
        Task<ICollection<Card>> GetByUserStoryId(int userStoryId);
        Task Update(Card card);

    }
}
