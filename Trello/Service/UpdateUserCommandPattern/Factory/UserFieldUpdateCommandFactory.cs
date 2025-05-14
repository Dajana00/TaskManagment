using Microsoft.AspNetCore.Identity;
using Trello.Service.UpdateUserCommandPattern.Interface;
using Trello.Service.UpdateUserCommands.Commands;

namespace Trello.Service.UpdateUserCommandPattern.Factory
{
    public static class UserFieldUpdateCommandFactory
    {
        public static IUserFieldUpdateCommand? GetCommand(string fieldName, IServiceProvider serviceProvider)
        {
            return fieldName.ToLower() switch
            {
                "firstname" => serviceProvider.GetService<FirstNameUpdateCommand>(),
                "lastname" => serviceProvider.GetService<LastNameUpdateCommand>(),
                "email" => serviceProvider.GetService<EmailUpdateCommand>(),
                "username" => serviceProvider.GetService<UsernameUpdateCommand>(), 
                "phonenumber" => serviceProvider.GetService<PhoneNumberUpdateCommand>(),
                _ => null
            };
        }
    }
}
