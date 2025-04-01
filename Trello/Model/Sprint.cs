namespace Trello.Model
{
    public enum SprintStatus {Backlog, Active, Completed, Archived }
    public class Sprint
    {
        public int Id {  get; set; }    
        public string Name { get; set; }
        public SprintStatus Status { get; set; } 

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int ProjectId { get; set; }  // Sprint pripada projektu
        public Project Project { get; set; }
        public Sprint() { } 
    }
}
