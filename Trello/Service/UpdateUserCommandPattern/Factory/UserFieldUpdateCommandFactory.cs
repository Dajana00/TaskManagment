using Trello.Service.UpdateUserCommandPattern.Interface;
using Trello.Service.UpdateUserCommands.Commands;

namespace Trello.Service.UpdateUserCommandPattern.Factory
{
    public static class UserFieldUpdateCommandFactory
    {
        private static readonly Dictionary<string, IUserFieldUpdateCommand> _commands = new()
    {
        { "firstname", new FirstNameUpdateCommand() },
        { "lastname", new LastNameUpdateCommand() },
        { "email", new EmailUpdateCommand() },
        { "username", new UsernameUpdateCommand() },
        { "phonenumber", new PhoneNumberUpdateCommand() }
    };

        public static IUserFieldUpdateCommand? GetCommand(string fieldName)
        {
            _commands.TryGetValue(fieldName.ToLower(), out var command);
            return command;
        }
    }

}
