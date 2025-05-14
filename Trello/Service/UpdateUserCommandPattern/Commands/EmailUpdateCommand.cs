using Trello.Model;
using Trello.Service.UpdateUserCommandPattern.Interface;

namespace Trello.Service.UpdateUserCommands.Commands
{
    public class EmailUpdateCommand : IUserFieldUpdateCommand
    {
        public Task ApplyAsync(User user, string newValue)
        {
            user.Email = newValue;
            return Task.CompletedTask;

        }
    }
}
