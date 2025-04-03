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

    }
}