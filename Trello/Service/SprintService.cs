using AutoMapper;
using FluentResults;
using Trello.DTOs;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;

namespace Trello.Service
{
    public class SprintService : ISprintService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _sprintMapper;


        public SprintService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _sprintMapper = mapper;
        }

        public async Task<Result<SprintDto>> CreateAsync(SprintDto sprintDto)
        {
            if (sprintDto == null)
                return Result.Fail("Sprint to create cannot be null.");

            if (string.IsNullOrWhiteSpace(sprintDto.Name))
                return Result.Fail("Sprint name is required.");


            var sprint = _sprintMapper.Map<Sprint>(sprintDto);

            await _unitOfWork.Sprints.CreateAsync(sprint);
            await _unitOfWork.SaveAsync();

            return Result.Ok(_sprintMapper.Map<SprintDto>(sprint));
        }
        public async Task<Result<ICollection<SprintDto>>> GetByProjectId(int id)
        {
            try
            {
                var sprints = await _unitOfWork.Sprints.GetByProjectId(id);

                if (sprints == null)
                    return Result.Fail($"No sprints found with project id: {id}");


                var sprintDtos = _sprintMapper.Map<ICollection<SprintDto>>(sprints);
                return Result.Ok(sprintDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving cards with user story id {id} : {ex.Message}");
            }
        }
    }
}
