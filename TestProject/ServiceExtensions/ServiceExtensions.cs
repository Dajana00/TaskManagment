using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Data;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Repository;
using Trello.Service.IService;
using Trello.Service;
using Trello.Validation.SprintValidator;
using Trello.Mapper;

namespace TestProject.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            // Registracija InMemory baze
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IBacklogRepository, BacklogRepository>();
            services.AddScoped<IUserStoryRepository, UserStoryRepository>();
            services.AddScoped<ISprintRepository, SprintRepository>();
            services.AddScoped<IUserCardRepository, UserCardRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // AutoMapper
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Mockovani servisi koje koristi SprintService
            services.AddScoped(_ => Mock.Of<IBoardService>());
            services.AddScoped(_ => Mock.Of<ICardService>());
            services.AddScoped(_ => Mock.Of<IUserStoryService>());
            services.AddScoped(_ => Mock.Of<IBacklogService>());

            // SprintValidator factory
            services.AddScoped<Func<IEnumerable<Sprint>, SprintValidator>>(provider =>
                sprints => new SprintValidator(sprints));

            services.AddScoped<SprintService>();
            services.AddLogging();


            return services.BuildServiceProvider();
        }
    }
}
