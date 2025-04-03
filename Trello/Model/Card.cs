namespace Trello.Model
{
    public enum CardStatus { Backlog, ToDo, InProgress, QA, Done, Archived}
    public class Card
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public CardStatus Status { get; set; } = CardStatus.Backlog;
        public DateTime DueDate { get; set; }
      

        public Sprint? Sprint { get; set; }
        public int? SprintId { get; set; }

        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
        public ICollection<UserCard>? UserCards { get; set; } = new HashSet<UserCard>();

        public Card() 
        { 
            
        }
    }
}
