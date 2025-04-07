using Trello.DTOs;
using Trello.Model;

namespace Trello.Mapper
{
    public class BacklogMapper
    {
        public BacklogDto CreateDto(Backlog backlog)
        {
            return new BacklogDto
            {
                Id = backlog.Id,
                ProjectId = backlog.ProjectId,
               
            };
        }

        public ICollection<BacklogDto> CreateDtoList(ICollection<Backlog> backlogs)
        {
            return backlogs.Select(CreateDto).ToList();
        }
    }
}
