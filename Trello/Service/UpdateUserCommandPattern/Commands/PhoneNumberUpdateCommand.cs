using Trello.Model;
using Trello.Service.UpdateUserCommandPattern.Interface;

namespace Trello.Service.UpdateUserCommands.Commands
{
    public class PhoneNumberUpdateCommand : IUserFieldUpdateCommand
    {
        public Task ApplyAsync(User user, string newValue)
        {
            user.PhoneNumber = newValue;
            return Task.CompletedTask;

        }
    }
}
