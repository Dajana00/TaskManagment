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

        public CardSprintService(
            ICardService cardService,
            ISprintService sprintService,
            IProjectService projectService, IMapper mapper)
        {
            _cardService = cardService;
            _sprintService = sprintService;
            _projectService = projectService;
            _mapper = mapper;   
        }
        public async Task<Result> AddCardToActiveSprint(int cardId)
        {
            if (cardId == 0)
                return Result.Fail("Invalid id.");

            var card = await _cardService.GetByIdAsync(cardId);
            if (card == null)
                return Result.Fail($"Card with ID {cardId} not found.");

            var project = _projectService.GetByUserStory(card.Value.Id);
            if (project == null)
            {
                return Result.Fail("Cannot add to acitve sprint. Cannot find project from user story od this card");
            }
            var activeSprint = _sprintService.GetActiveByProjectId(project.Result.Value.Id);

            card.Value.SprintId = activeSprint.Id;
            card.Value.Status = CardStatus.ToDo;

            var result = await _cardService.UpdateAsync(_mapper.Map<Card>(card));

            return Result.Ok();
        }
    }
}
