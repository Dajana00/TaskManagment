using Trello.Model;

namespace Trello.Repository.IRepository
{
    public interface IColumnRepository
    {
        Task CreateAsync(Column column);
    }
}
