using Trello.DTOs;
using Trello.Model;

namespace Trello.Mapper
{
    public class ColumnMapper
    {
        public ColumnDto CreateDto(Column column)
        {
            return new ColumnDto
            {
                Id = column.Id,
                Name = column.Name,
                BoardId = column.BoardId,
                //CardIds = column.Cards.Select(c => c.Id).ToList()
            };
        }

        public ICollection<ColumnDto> CreateDtoList(ICollection<Column> columns)
        {
            return columns.Select(CreateDto).ToList();
        }
    }

}
