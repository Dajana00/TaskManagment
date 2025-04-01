using Trello.DTOs;
using Trello.Model;

namespace Trello.Mapper
{
    public class ProjectMapper
    {
        //private readonly UserMapper _userMapper;
        //private readonly BoardMapper _boardMapper;
        //private readonly SprintMapper _sprintMapper;

        public ProjectMapper()
        {
            //_userMapper = new UserMapper();
            //_boardMapper = new BoardMapper();
            //_sprintMapper = new SprintMapper();
        }

        public ProjectDetailsDto CreateDto(Project project)
        {
            return new ProjectDetailsDto
            {
                Id = project.Id,
                Name = project.Name,
                OwnerId = project.OwnerId,
                BoardId = project.BoardId,
                //UserIds = project.UserProjects.Select(up => up.User.Id).ToList(),
                //BoardId = project.Board.Id,
                //SprintIds = project.Sprints.Select(s => s.Id).ToList()
            };
        }

        public List<ProjectDetailsDto> CreateDtoList(ICollection<Project> projects)
        {
            return projects.Select(CreateDto).ToList();
        }
    }



}
