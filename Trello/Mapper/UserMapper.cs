using Trello.DTOs;
using Trello.Model;

namespace Trello.Mapper
{
    public class UserMapper
    {
        private ProjectMapper _projectMapper;

        public UserMapper()
        {
            _projectMapper = new ProjectMapper();
        }

        public UserDto CreateDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                ProjectIds = user.UserProjects.Select(up => up.Project.Id).ToList()
            };
        }

        public List<UserDto> CreateDtoList(List<User> users)
        {
            return users.Select(CreateDto).ToList();
        }
    }

}
