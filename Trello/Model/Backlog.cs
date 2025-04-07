using System.Security.Cryptography;

namespace Trello.Model
{
    public class Backlog
    {
        public int Id { get; set; }
        public ICollection<UserStory> UserStories { get; set; } = new HashSet<UserStory>();
        public Project Project { get; set; }
        public int ProjectId;



        public Backlog() { }
    }
}
