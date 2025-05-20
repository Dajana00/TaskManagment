using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Trello.Data;
using Trello.DTOs;
using Trello.Service;
using Trello.Mapper;
using Trello.Model;

namespace TestProject
{
    public class SprintServiceIntegrationTest
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public SprintServiceIntegrationTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });
            _mapper = config.CreateMapper();

            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateSprint_WhenValid()
        {
            var serviceProvider = ServiceExtensions.ServiceExtensions.BuildServiceProvider();

            // Scope simulira HttpRequest u ASP.NET
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var service = scope.ServiceProvider.GetRequiredService<SprintService>();

            context.Projects.Add(new Project { Id = 1, Name = "Test Project" });
            await context.SaveChangesAsync();

            var sprintDto = new SprintDto
            {
                Name = "Sprint 1",
                ProjectId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };

            var result = await service.CreateAsync(sprintDto);

            Assert.True(result.IsSuccess);
            Assert.Equal("Sprint 1", result.Value.Name);
        }

       

    }

}
