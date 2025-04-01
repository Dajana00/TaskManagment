namespace Trello.Model
{
    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Column> Columns { get; set; } = new HashSet<Column>();
        //sadrzi user story , koji sadrzi vise taskova
        //task sadrzi property sattsusa da li je to do , in progress itd

        public Project Project { get; set; }    
        public int ProjectId {  get; set; }

        public int? ActiveSprintId { get; set; }  
        public Sprint? ActiveSprint { get; set; } 

        public Board() { }
    }
}
