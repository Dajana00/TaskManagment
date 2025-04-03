using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface ICardRepository
    {
        Task CreateAsync(Card card);
        Task<Card?> GetByIdAsync(int cardId);
        void Update(Card card);

    }
}
