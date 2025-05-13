using Trello.Model;

namespace Trello.Service.UpdateUserCommandPattern.Interface
{
    public interface IUserFieldUpdateCommand
    {
        void Apply(User user, string newValue);
    }
}
