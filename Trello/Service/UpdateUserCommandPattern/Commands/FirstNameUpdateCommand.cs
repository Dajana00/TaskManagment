using Trello.Model;
using Trello.Service.UpdateUserCommandPattern.Interface;

namespace Trello.Service.UpdateUserCommands.Commands
{
    public class FirstNameUpdateCommand : IUserFieldUpdateCommand
    {
        public Task ApplyAsync(User user, string newValue)
        {
            user.FirstName = newValue;
            return Task.CompletedTask;

        }
    }
}
