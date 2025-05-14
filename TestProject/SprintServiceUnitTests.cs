using AutoMapper;
using Trello.DTOs;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;
using Trello.Service;
using Moq;
using FluentResults;
using Trello.Validation.SprintValidator;

namespace TestProject.Tests
{
    public class ActivateSprintTest
    {
        [Fact]
        public async Task Activate_ShouldReturnOkResult_WhenSprintIsActivatedSuccessfully()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockSprintRepo = new Mock<ISprintRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockBoardService = new Mock<IBoardService>();
            var mockCardService = new Mock<ICardService>();

            var sprint = new Sprint { Id = 1 };
            var sprintDto = new SprintDto { Id = 1 };

            mockSprintRepo.Setup(r => r.Activate(1)).ReturnsAsync(sprint);
            mockSprintRepo.Setup(r => r.GetById(1)).ReturnsAsync(sprint);
            mockUnitOfWork.Setup(u => u.Sprints).Returns(mockSprintRepo.Object);

            mockBoardService.Setup(b => b.AddSprintToBoard(It.IsAny<Sprint>())).ReturnsAsync(Result.Ok());
            mockMapper.Setup(m => m.Map<SprintDto>(It.IsAny<Sprint>())).Returns(sprintDto);

            var sprintService = new SprintService(
                mockUnitOfWork.Object,
                mockMapper.Object,
                mockBoardService.Object,
                mockCardService.Object,
                null, // userStoryService nije potreban ovde
                null, // backlogService nije potreban ovde
                null  // validator nije potreban za Activate
            );

            // Act
            var result = await sprintService.Activate(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact]
        public async Task Complete_ShouldReturnFail_WhenNotAllCardsAreDone()
        {
            var mockUow = new Mock<IUnitOfWork>();
            var mockCardService = new Mock<ICardService>();
            var mockBoardService = new Mock<IBoardService>();
            var mockMapper = new Mock<IMapper>();

            mockCardService.Setup(s => s.AreAllCardsDone(1)).ReturnsAsync(false);

            var service = new SprintService(
                mockUow.Object,
                mockMapper.Object,
                mockBoardService.Object,
                mockCardService.Object,
                null,
                null,
                _ => new SprintValidator(new List<Sprint>()) // dummy validator
            );

            var result = await service.Complete(1);

            Assert.True(result.IsFailed);
            Assert.Equal("Cannot complete sprint. All cards must be done.", result.Errors[0].Message);
        }

    }
}