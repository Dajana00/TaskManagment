using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task<bool> UserExistsAsync(string email, string username);
        Task SaveUserAsync(User user);
        Task<ICollection<User>> GetByProjectIdAsync(int projectId);
        Task<ICollection<User>> GetNonMembers(int projectId);
        Task AddUserToProjectAsync(int projectId, int userId);
    }

}
