using Trello.Model;

namespace Trello.DTOs
{
    public class CardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public CardStatus Status { get; set; }
        //recordi 
    }
}
