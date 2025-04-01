using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface IBoardRepository
    {
        Task CreateAsync(Board board);
        Task<Board> GetById(int id);
    }
}
