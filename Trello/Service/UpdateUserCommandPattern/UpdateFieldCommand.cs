using MediatR;
using Trello.DTOs;


namespace Trello.Service.UpdateUserCommands
{

    public class UpdateUserFieldCommand : IRequest<UserDto?>
    {
        public int UserId { get; set; }
        public Dictionary<string, string> FieldsToUpdate { get; set; } = new();
    }

}
