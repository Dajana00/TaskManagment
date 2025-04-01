using Trello.DTOs;
using Trello.Model;

namespace Trello.Mapper
{
    public class BoardMapper
    {
       // private readonly ColumnMapper _columnMapper;
        private readonly SprintMapper _sprintMapper;
        private readonly ProjectMapper _projectMapper;
        private readonly ColumnMapper _columnMapper;

        public BoardMapper()
        {
            _columnMapper = new ColumnMapper();
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
                Columns = _columnMapper.CreateDtoList(board.Columns),
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
