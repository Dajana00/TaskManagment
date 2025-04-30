using Trello.Model;

namespace Trello.DTOs
{
    public class MoveCardDto
    {
        public int CardId {  get; set; }
        public string NewStatus { get; set; }    
    }
}
