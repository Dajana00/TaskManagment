using Trello.Repository.IRepository;
using Trello.Repository;
using Trello.Service.Iservice;
using Trello.Service.IService;
using Trello.Service;

namespace Trello.Helpers
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Repozitorijumi
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IBacklogRepository, BacklogRepository>();
            services.AddScoped<IUserStoryRepository, UserStoryRepository>();
            services.AddScoped<ISprintRepository, SprintRepository>();
            //singlton da provjerim ovde


            // Servisi
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IBacklogService, BacklogService>();
            services.AddScoped<IUserStoryService, UserStoryService>();
            services.AddScoped<ISprintService, SprintService>();


            // Konfiguracija za JWT
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        }
    }
}
