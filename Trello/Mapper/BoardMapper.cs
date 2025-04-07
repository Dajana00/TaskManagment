using Trello.DTOs;
using Trello.Model;

namespace Trello.Mapper
{
    public class BoardMapper
    {
        private readonly SprintMapper _sprintMapper;
        private readonly ProjectMapper _projectMapper;

        public BoardMapper()
        {
            _sprintMapper = new SprintMapper();
            _projectMapper = new ProjectMapper();
        }

        public BoardDto CreateDto(Board board)
        {
            return new BoardDto
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                ProjectId = board.ProjectId,
                ActiveSprintId = board.ActiveSprintId,
            };
        }

        public ICollection<BoardDto> CreateDtoList(ICollection<Board> boards)
        {
            return boards.Select(CreateDto).ToList();
        }
    }

}
