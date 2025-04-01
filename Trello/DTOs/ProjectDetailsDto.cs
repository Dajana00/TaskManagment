namespace Trello.DTOs
{
    public class ProjectDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public int BoardId { get; set; }
        //public UserDto Owner { get; set; } 
        //public List<UserDto> Users { get; set; } = new List<UserDto>(); // Lista korisnika u projektu
        //public ICollection<int> UserIds { get; set; } = new List<int>(); // Lista korisnika u projektu
        //public List<SprintDto> Sprints { get; set; } = new List<SprintDto>(); // Svi sprintovi
        //public List<int> SprintIds { get; set; } = new List<int>(); // Svi sprintovi
        //public BoardDto Board { get; set; } // Informacije o tabli
        //public int BoardId { get; set; }    
    }

}
