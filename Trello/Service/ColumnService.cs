using FluentResults;
using Trello.Mapper;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;

namespace Trello.Service
{
    public class ColumnService : IColumnService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BoardMapper _boardMapper;

        public ColumnService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _boardMapper = new BoardMapper();
        }

        public async Task<Result<ICollection<Column>>> CreateDefaultBoardColumnsAsync(Board board)
        {
            if (board == null)
                return Result.Fail("Cannot create column becuase board is null");

            var defaultColumnNames = new List<ColumnName>
            {
                ColumnName.Backlog,
                ColumnName.ToDo,
                ColumnName.QA,
                ColumnName.Done
            };

            ICollection<Column> createdColumns = new List<Column>();

            foreach (var name in defaultColumnNames)
            {
                var column = new Column
                {
                    Name = name,
                    Board = board,
                    BoardId = board.Id
                };

                await _unitOfWork.Columns.CreateAsync(column);
                createdColumns.Add(column);
            }

            await _unitOfWork.SaveAsync();

            return Result.Ok(createdColumns);
        }

    
    }
}