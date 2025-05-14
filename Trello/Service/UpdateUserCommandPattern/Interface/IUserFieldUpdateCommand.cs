using Trello.Model;

namespace Trello.Service.UpdateUserCommandPattern.Interface
{
    public interface IUserFieldUpdateCommand
    {
        Task ApplyAsync(User user, string newValue);
    }
}
