using FluentResults;
using System.Threading.Tasks;
using Trello.DTOs;
using Trello.Model;

namespace Trello.Service.IService
{
    public interface IUserCardService
    {

        Task<Result> AddUserToCardAsync(int cardId, int userId);
        Task<Result<ICollection<UserDto>>> GetNonMembers(int cardId, int projectId);
        Task<Result<ICollection<UserDto>>> GetCardMembersAsync(int cardId);
    }
}
