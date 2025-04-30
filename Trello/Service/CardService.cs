using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.SignalR;
using Trello.DTOs;
using Trello.Mapper;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;
using Trello.Validation;
using Trello.WebSocket;

namespace Trello.Service
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserStoryService _userStoryService;
        private readonly IBacklogService _backlogService;
        private readonly IProjectService _projectService;
        private readonly CreateCardValidator _createCardValidator;  



        public CardService(IUnitOfWork unitOfWork, 
            IMapper mapper,
            IUserStoryService userStoryService,
            IBacklogService backlogService, 
            IProjectService projectService,
            CreateCardValidator createCardValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userStoryService = userStoryService;   
            _backlogService = backlogService;  
            _projectService = projectService;   
            _createCardValidator = createCardValidator; 
        }

        public async Task<Result<CardDto>> CreateAsync(CreateCardDto cardDto)
        {
            _createCardValidator.Validate(cardDto);
            var card = _mapper.Map<Card>(cardDto);

            await _unitOfWork.Cards.CreateAsync(card);
            await _unitOfWork.SaveAsync();

            return Result.Ok(_mapper.Map<CardDto>(card));
        }

        public async Task<Result> UpdateCardStatus(MoveCardDto moveCardDto)
        {
            if (moveCardDto == null)
                return Result.Fail("Invalid input data.");

            var card = await _unitOfWork.Cards.GetByIdAsync(moveCardDto.CardId);
            if (card == null)
                return Result.Fail($"Card with ID {moveCardDto.CardId} not found.");

            card.Status = ConvertToCardStatus(moveCardDto.NewStatus); 
            await _unitOfWork.Cards.Update(card);

            return Result.Ok();
        }
        public async Task<Result<ICollection<CardDto>>> GetByBoardId(int boardId)
        {
            try
            {
                var board = await _unitOfWork.Boards.GetById(boardId);
                if (board == null)
                {
                    return Result.Fail($"No board found with id: {boardId}");
                }
                if (board.ActiveSprintId != null)
                {
                    var cards = await _unitOfWork.Cards.GetByActiveSprint((int)board.ActiveSprintId);
                    if (cards == null || cards.Count == 0)
                        return Result.Fail("No cards found.");
                    var cardDtos = cards
                    .Select(us => _mapper.Map<CardDto>(us))
                    .ToList();

                    return Result.Ok((ICollection<CardDto>)cardDtos);
                }
                return Result.Fail("No cards in this active sprint");
                
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving user stories: {ex.Message}");
            }
        }

        public CardStatus ConvertToCardStatus(string status)
        {
            return status switch
            {
                "ToDo" => CardStatus.ToDo,
                "InProgress" => CardStatus.InProgress,
                "QA" => CardStatus.QA,
                "Done" => CardStatus.Done,
                "Archived" => CardStatus.Archived,
                _ => CardStatus.Backlog,  // Ako status nije validan, postavi kao Backlog
            };
        }

        public async Task<Result<ICollection<CardDto>>> GetByUserStoryId(int id)
        {
            try
            {
                var userStories = await _unitOfWork.Cards.GetByUserStoryId(id);

                if (userStories == null || userStories.Count == 0)
                    return Result.Fail($"No cards found with user story id: {id}");

                var userStoryDtos = userStories
                    .Select(us => _mapper.Map<CardDto>(us))
                    .ToList();

                return Result.Ok((ICollection<CardDto>)userStoryDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving cards with user story id {id} : {ex.Message}");
            }
        }


        public async Task<bool> AreAllCardsDone(int boardId)
        {
            var result = await GetByBoardId(boardId);
            if (result.IsFailed || result.Value == null)
                return false;

            return result.Value.All(card => card.Status == CardStatus.Done);
        }

        public async Task<Result<CardDto>> GetByIdAsync(int id)
        {
            var card = await _unitOfWork.Cards.GetByIdAsync(id);
            return _mapper.Map<CardDto>(card);  
        }
        public async Task<Result> UpdateAsync(Card card)
        {
            if (card == null)
                return Result.Fail("Card cannot be null.");
            try
            {
                await _unitOfWork.Cards.Update(card);
                
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to update card: {ex.Message}");
            }
        }

    }
}