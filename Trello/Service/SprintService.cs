using AutoMapper;
using FluentResults;
using Trello.DTOs;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;
using Trello.Validation.SprintValidator;

namespace Trello.Service
{
    public class SprintService : ISprintService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _sprintMapper;
        private readonly IBoardService _boardService;  
        private readonly ICardService _cardService;
        private readonly Func<IEnumerable<Sprint>, SprintValidator> _sprintValidatorFactory;

        public SprintService(IUnitOfWork unitOfWork, IMapper mapper, IBoardService boardService,
            ICardService cardService, IUserStoryService userStoryService,
            IBacklogService backlogService, Func<IEnumerable<Sprint>, SprintValidator> sprintValidatorFactory)
        {
            _unitOfWork = unitOfWork;
            _sprintMapper = mapper;
            _boardService = boardService; 
            _cardService = cardService;
            _sprintValidatorFactory = sprintValidatorFactory;
        }

        public async Task<Result<SprintDto>> Activate(int id)
        {
            try
            {
                if (id == 0)
                    return Result.Fail($"Cannot activate sprint. Id is invalid.");
                var sprint = await _unitOfWork.Sprints.Activate(id);
                var activatedSprint = await GetById(id);
                var boardResult = await _boardService.AddSprintToBoard(activatedSprint);
                if (boardResult.IsFailed)
                    return Result.Fail<SprintDto>($"Sprint activated, but failed to update board: {boardResult.Errors[0].Message}");
                return Result.Ok(_sprintMapper.Map<SprintDto>(sprint));
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred during activating sprint : {ex.Message}");
            }
        }
        public async Task<Result<SprintDto>> Complete(int boardId)
        {
            try
            {
                if (boardId == 0)
                    throw new Exception($"Cannot complete sprint. Board id is invalid.");
                var allCardsDone = await _cardService.AreAllCardsDone(boardId);
                if (!allCardsDone)
                    return Result.Fail("Cannot complete sprint. All cards must be done.");
                var board = await _boardService.GetById(boardId);
              
                var sprintId = board.Value.ActiveSprintId;
                var sprint = await _unitOfWork.Sprints.Complete((int)sprintId);

                board.Value.ActiveSprintId = null;
                await _unitOfWork.SaveAsync();
                return Result.Ok(_sprintMapper.Map<SprintDto>(sprint));
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred during completing sprint : {ex.Message}");
            }

        }
        public async Task<Result<SprintDto>> CreateAsync(SprintDto sprintDto)
        {
            var existingSprints = await GetByProjectId(sprintDto.ProjectId);
            var mappedSprints = _sprintMapper.Map<ICollection<Sprint>>(existingSprints.Value);

            var sprintValidator = _sprintValidatorFactory(mappedSprints);
            var errors = sprintValidator.Validate(sprintDto);

            if (errors.Any())
                return Result.Fail(errors.First());

            var sprint = _sprintMapper.Map<Sprint>(sprintDto);

            await _unitOfWork.Sprints.CreateAsync(sprint);
            await _unitOfWork.SaveAsync();

            return Result.Ok(_sprintMapper.Map<SprintDto>(sprint));
        }

       
        public async Task<Sprint> GetById(int id)
        {
            return await _unitOfWork.Sprints.GetById(id);
        }

        public async Task<Result<ICollection<SprintDto>>> GetByProjectId(int id)
        {
            try
            {
                var sprints = await _unitOfWork.Sprints.GetByProjectId(id);

                if (sprints == null)
                    return Result.Fail($"No sprints found with project id: {id}");


                var sprintDtos = _sprintMapper.Map<ICollection<SprintDto>>(sprints);
                return Result.Ok(sprintDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving cards with user story id {id} : {ex.Message}");
            }
        }

        public async Task<Result<SprintDto>> GetActiveByProjectId(int id)
        {
            try
            {
                var sprints = await _unitOfWork.Sprints.GetActiveByProjectId(id);

                if (sprints == null)
                    return Result.Fail($"No active sprint found with project id: {id}");


                var sprintDto = _sprintMapper.Map<SprintDto>(sprints);
                return Result.Ok(sprintDto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving cards with user story id {id} : {ex.Message}");
            }
        }

      
    }
}
