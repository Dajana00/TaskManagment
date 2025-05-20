using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface IUserCardRepository
    {
        Task AddUserToCardAsync(int cardId, int userId);
        Task<ICollection<User>> GetNonMembers(int cardId, int projectId);
        Task<ICollection<User>> GetCardMembersAsync(int cardId);
    }
}
