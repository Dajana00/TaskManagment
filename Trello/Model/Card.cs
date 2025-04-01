﻿namespace Trello.Model
{
    public class Card
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Column Column { get; set; }  
        public int ColumnId { get; set; }

        public Sprint Sprint { get; set; }
        public int SprintId { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<UserCard> UserCards { get; set; } = new HashSet<UserCard>();

        public Card() { }
    }
}
