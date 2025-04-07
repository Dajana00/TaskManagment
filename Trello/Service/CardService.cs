using FluentResults;
using Trello.DTOs;
using Trello.Mapper;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;

namespace Trello.Service
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CardMapper _cardMapper;


        public CardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _cardMapper = new CardMapper();
            
        }

        public async Task<Result<CardDto>> CreateAsync(CardDto cardDto)
        {
            if (cardDto == null)
                return Result.Fail("Card to create cannot be null.");

            if (string.IsNullOrWhiteSpace(cardDto.Title))
                return Result.Fail("Card title is required.");

            if (string.IsNullOrWhiteSpace(cardDto.Description))
                return Result.Fail("Card description is required.");


            var card = new Card
            {
                Title = cardDto.Title,
                Description = cardDto.Description,
                DueDate = cardDto.DueDate,
                Status = CardStatus.Backlog
            };

            await _unitOfWork.Cards.CreateAsync(card);
            await _unitOfWork.SaveAsync();

            return Result.Ok(_cardMapper.CreateDto(card));
        }

        public async Task<Result> UpdateCardStatus(MoveCardDto moveCardDto)
        {
            if (moveCardDto == null)
                return Result.Fail("Invalid input data.");

            var card = await _unitOfWork.Cards.GetByIdAsync(moveCardDto.CardId);
            if (card == null)
                return Result.Fail($"Card with ID {moveCardDto.CardId} not found.");

            card.Status = moveCardDto.NewStatus; 
            _unitOfWork.Cards.Update(card);
            await _unitOfWork.SaveAsync();

            return Result.Ok();
        }
        public async Task<Result<ICollection<CardDto>>> GetAll()
        {
            try
            {
                var userStories = await _unitOfWork.Cards.GetAll();

                if (userStories == null || userStories.Count == 0)
                    return Result.Fail("No cards found.");

                var userStoryDtos = userStories
                    .Select(us => _cardMapper.CreateDto(us))
                    .ToList();

                return Result.Ok((ICollection<CardDto>)userStoryDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving user stories: {ex.Message}");
            }
        }
    }
}