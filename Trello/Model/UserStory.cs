namespace Trello.Model
{
    public class UserStory
    {
        public int Id { get; set; } 
        public string Title { get; set; }  
        public string Description { get; set; } 
        public Backlog Backlog { get; set; }
        public int BacklogId {  get; set; } 
        public UserStory() { }  

    }
}
