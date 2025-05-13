using MediatR;
using Trello.Data;
using Trello.DTOs;
using Trello.Service.UpdateUserCommandPattern.Factory;

namespace Trello.Service.UpdateUserCommands
{
    public class UpdateUserFieldCommandHandler : IRequestHandler<UpdateUserFieldCommand, UserDto>
    {
        private readonly AppDbContext _context;

        public UpdateUserFieldCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(UpdateUserFieldCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return null;

            foreach (var field in request.FieldsToUpdate)
            {
                var command = UserFieldUpdateCommandFactory.GetCommand(field.Key);
                command?.Apply(user, field.Value);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}