namespace Trello.Model
{
    public class Comment
    {
        public int Id { get; set; } 
        public string Text {  get; set; }
        public DateTime CreatedAt { get; set; }

        public User Author { get; set; }  
        public int AuthorId { get; set; } 

        public Card Card { get; set; }   
        public int CardId { get; set; }  
    }
}
