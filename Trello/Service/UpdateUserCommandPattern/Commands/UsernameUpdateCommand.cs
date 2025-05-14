using Microsoft.AspNetCore.Identity;
using Trello.Model;
using Trello.Service.UpdateUserCommandPattern.Interface;

namespace Trello.Service.UpdateUserCommands.Commands
{
    public class UsernameUpdateCommand : IUserFieldUpdateCommand
    {
        private readonly UserManager<User> _userManager;

        public UsernameUpdateCommand(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task ApplyAsync(User user, string newValue)
        {
            var result = await _userManager.SetUserNameAsync(user, newValue);
        }
    }

}
