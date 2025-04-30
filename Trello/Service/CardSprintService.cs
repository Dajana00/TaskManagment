using AutoMapper;
using FluentResults;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;

namespace Trello.Service
{
    public class CardSprintService : ICardSprintService
    {
        private readonly ICardService _cardService;
        private readonly ISprintService _sprintService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CardSprintService(
            ICardService cardService,
            ISprintService sprintService,
            IProjectService projectService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _cardService = cardService;
            _sprintService = sprintService;
            _projectService = projectService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> AddCardToActiveSprint(int cardId)
        {
            if (cardId == 0)
                return Result.Fail("Invalid id.");

            var card = await _cardService.GetByIdAsync(cardId);
            if (card == null)
                return Result.Fail($"Card with ID {cardId} not found.");

            var existingEntity = await _unitOfWork.Cards.GetByIdAsync(card.Value.Id);
            if (existingEntity == null)
                return Result.Fail("Card not found.");
            var project = await _projectService.GetByUserStory(card.Value.UserStoryId);
            if (project == null)
            {
                return Result.Fail("Cannot add to acitve sprint. Cannot find project from user story od this card");
            }
            var activeSprint = await _sprintService.GetActiveByProjectId(project.Value.Id);

            existingEntity.SprintId = activeSprint.Value.Id;
            existingEntity.Status = CardStatus.ToDo;

            await _unitOfWork.Cards.Update(existingEntity);

            return Result.Ok();
        }
    }
}
