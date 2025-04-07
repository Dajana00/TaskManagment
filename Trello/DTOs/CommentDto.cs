using Trello.Model;

namespace Trello.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        //public User Author { get; set; }
        public int AuthorId { get; set; }

        //public Card Card { get; set; }
        public int CardId { get; set; }
    }
}
