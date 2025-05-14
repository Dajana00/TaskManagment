using Trello.Model;
using Trello.Service.UpdateUserCommandPattern.Interface;

namespace Trello.Service.UpdateUserCommands.Commands
{
    public class LastNameUpdateCommand : IUserFieldUpdateCommand
    {
        public Task ApplyAsync(User user, string newValue)
        {
            user.LastName = newValue;
            return Task.CompletedTask;

        }
    }
}
