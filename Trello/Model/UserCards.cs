namespace Trello.Model
{
    public class UserCard  
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; }

        public DateTime AssignedAt { get; set; }  
    }
}
