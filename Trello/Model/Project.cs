namespace Trello.Model
{
    public class Project
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int OwnerId {  get; set; }   
        public User Owner { get; set; }
        public ICollection<UserProject> UserProjects { get; set; } = new HashSet<UserProject>();
        public Board Board { get; set; }
        public int BoardId { get; set; }
        public ICollection<Sprint> Sprints { get; set; }  = new HashSet<Sprint>();    
        public Project() { }

    }
}
