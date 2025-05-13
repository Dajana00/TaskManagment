using Trello.Model;
using Trello.Service.UpdateUserCommandPattern.Interface;

namespace Trello.Service.UpdateUserCommands.Commands
{
    public class FirstNameUpdateCommand : IUserFieldUpdateCommand
    {
        public void Apply(User user, string newValue)
        {
            user.FirstName = newValue;
        }
    }
}
