using MediatR;
using Trello.Data;
using Trello.DTOs;
using Trello.Service.UpdateUserCommandPattern.Factory;
using Trello.Service.UpdateUserCommands;

namespace Trello.Service.UpdateUserCommandPattern
{

    public class UpdateUserFieldCommandHandler : IRequestHandler<UpdateUserFieldCommand, UserDto>
    {
        private readonly AppDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public UpdateUserFieldCommandHandler(AppDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task<UserDto> Handle(UpdateUserFieldCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return null;

            foreach (var field in request.FieldsToUpdate)
            {
                var command = UserFieldUpdateCommandFactory.GetCommand(field.Key, _serviceProvider);
                await command?.ApplyAsync(user, field.Value);
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