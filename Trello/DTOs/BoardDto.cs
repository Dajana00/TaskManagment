using Trello.Model;

namespace Trello.DTOs
{
    public class BoardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ColumnDto> Columns { get; set; } = new HashSet<ColumnDto>();
        public int ProjectId { get; set; }
        public int? ActiveSprintId { get; set; }
    
    }

}
