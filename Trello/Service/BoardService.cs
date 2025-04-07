using FluentResults;
using Trello.DTOs;
using Trello.Mapper;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;

namespace Trello.Service
{
    public class BoardService : IBoardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BoardMapper _boardMapper;

        public BoardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _boardMapper = new BoardMapper(); 
        }

        public async Task<Result<Board>> CreateDefaultBoardAsync(Project project)
        {
            if (project == null)
                return Result.Fail("Project cannot be null");

            var board = new Board
            {
                Name = $"{project.Name} Board",
                Description = "Default board",
                ProjectId = project.Id
            };

            await _unitOfWork.Boards.CreateAsync(board);
            _unitOfWork.SaveAsync();

            await _unitOfWork.SaveAsync();
            return Result.Ok(board);
        }
        public async Task<Result<BoardDto>> GetById(int id)
        {
            if (id <= 0)
                return Result.Fail("Invalid ID.");

            try
            {
                var board = await _unitOfWork.Boards.GetById(id);

                if (board == null)
                    return Result.Fail($"No board found with id: {id}.");


                return Result.Ok(_boardMapper.CreateDto(board));
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving user projects: {ex.Message}");
            }
        }
    }
}

