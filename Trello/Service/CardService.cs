using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.SignalR;
using Trello.DTOs;
using Trello.Mapper;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;
using Trello.WebSocket;

namespace Trello.Service
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;            
        }

        public async Task<Result<CardDto>> CreateAsync(CardDto cardDto)
        {
            if (cardDto == null)
                return Result.Fail("Card to create cannot be null.");

            if (string.IsNullOrWhiteSpace(cardDto.Title))
                return Result.Fail("Card title is required.");

            if (string.IsNullOrWhiteSpace(cardDto.Description))
                return Result.Fail("Card description is required.");
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
                    .Select(us => _mapper.Map<CardDto>(us))
                    .ToList();

                return Result.Ok((ICollection<CardDto>)userStoryDtos);
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

        public async Task<Result> AddToActiveSprint(int id)
        {
            if (id == 0)
                return Result.Fail("Invalid id.");

            var card = await _unitOfWork.Cards.GetByIdAsync(id);
            if (card == null)
                return Result.Fail($"Card with ID {id} not found.");

            //card.Sprint = 
            _unitOfWork.Cards.Update(card);
            await _unitOfWork.SaveAsync();

            return Result.Ok();
        }
    }
}