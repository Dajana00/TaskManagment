using AutoMapper;
using FluentResults;
using Trello.DTOs;
using Trello.Mapper;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;

namespace Trello.Service
{
    public class BacklogService : IBacklogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper  _backlogMapper;


        public BacklogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _backlogMapper = mapper;
        }

        public async Task<Result<Backlog>> CreateDefaultBacklogAsync(Project project)
        {
            if (project == null)
                return Result.Fail("Project cannot be null");

            var backlog = new Backlog
            { 
                ProjectId = project.Id
            };

            await _unitOfWork.Backlogs.CreateAsync(backlog);
            _unitOfWork.SaveAsync();

            await _unitOfWork.SaveAsync();
            return Result.Ok(backlog);
        }
        public async Task<Result<BacklogDto>> GetById(int id)
        {
            if (id <= 0)
                return Result.Fail("Invalid ID.");

            try
            {
                var backlog = await _unitOfWork.Backlogs.GetById(id);

                if (backlog == null)
                    return Result.Fail($"No backlog found with id: {id}.");


                return Result.Ok(_backlogMapper.Map<BacklogDto>(backlog));
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving user projects: {ex.Message}");
            }
        }
    }
}

