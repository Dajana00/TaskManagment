using Trello.Model;
using Trello.Service.UpdateUserCommandPattern.Interface;

namespace Trello.Service.UpdateUserCommands.Commands
{
    public class UsernameUpdateCommand : IUserFieldUpdateCommand
    {
        public void Apply(User user, string newValue)
        {
            user.UserName = newValue;
        }
    }
}
