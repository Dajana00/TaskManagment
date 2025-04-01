namespace Trello.Model
{
    public enum ColumnName { Backlog, ToDo, InProgress, QA, Done}
    public class Column
    {
        public int Id { get; set; }
        public ColumnName Name { get; set; }
        public int BoardId {  get; set; }   
        public Board Board { get; set; }

        public ICollection<Card> Cards { get; set; } = new HashSet<Card>();
        public Column() { }
    }
}
