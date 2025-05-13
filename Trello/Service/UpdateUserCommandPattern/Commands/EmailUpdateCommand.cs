using Trello.Model;
using Trello.Service.UpdateUserCommandPattern.Interface;

namespace Trello.Service.UpdateUserCommands.Commands
{
    public class EmailUpdateCommand : IUserFieldUpdateCommand
    {
        public void Apply(User user, string newValue)
        {
            user.Email = newValue;
        }
    }
}
