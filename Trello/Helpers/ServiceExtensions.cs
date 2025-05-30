﻿using Trello.Repository.IRepository;
using Trello.Repository;
using Trello.Service.Iservice;
using Trello.Service.IService;
using Trello.Service;
using Trello.Model;
using Trello.Validation.SprintValidator;
using Trello.Validation.UserStoryValidator;
using Trello.Validation.CardValidator;
using Trello.Service.UpdateUserCommands.Commands;

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
            services.AddScoped<IUserCardRepository, UserCardRepository>();

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
            services.AddScoped<ICardSprintService, CardSprintService>();
            services.AddScoped<IUserCardService, UserCardService>();
            //validatori
            services.AddScoped<Func<IEnumerable<Sprint>, SprintValidator>>(provider =>
            {
                return (existingSprints) => new SprintValidator(existingSprints);
            });
            services.AddScoped<CreateCardValidator>();
            services.AddScoped<CreateUserStoryValidator>();

            services.AddScoped<UsernameUpdateCommand>();
            services.AddScoped<PhoneNumberUpdateCommand>();
            services.AddScoped<EmailUpdateCommand>();
            services.AddScoped<FirstNameUpdateCommand>();
            services.AddScoped<LastNameUpdateCommand>();

            // Konfiguracija za JWT
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        }
    }
}
