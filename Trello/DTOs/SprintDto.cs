using Trello.Model;

namespace Trello.DTOs
{
    public class SprintDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SprintStatus Status { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int ProjectId { get; set; }  // Sprint pripada projektu
       // public ProjectDto Project { get; set; }
    }
}
