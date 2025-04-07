namespace Trello.DTOs
{
    public class UserStoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int BacklogId { get; set; }
    }
}
