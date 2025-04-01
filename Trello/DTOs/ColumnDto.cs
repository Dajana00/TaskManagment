using Trello.Model;

namespace Trello.DTOs
{
    public class ColumnDto
    {
        public int Id { get; set; }
        public ColumnName Name { get; set; }
        public int BoardId { get; set; }
        //public BoardDto Board { get; set; }

        //public ICollection<CardDto> Cards { get; set; } = new HashSet<CardDto>();
        //public ICollection<int> CardIds { get; set; } = new HashSet<int>();
    }
}
