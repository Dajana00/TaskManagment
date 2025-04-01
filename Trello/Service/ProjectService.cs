using FluentResults;
using Trello.DTOs;
using Trello.Mapper;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;

namespace Trello.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBoardService _boardService;   
        private readonly ProjectMapper _projectMapper;  
        

        public ProjectService(IUnitOfWork unitOfWork, IBoardService boardService)
        {
            _unitOfWork = unitOfWork;
            _boardService = boardService; 
            _projectMapper = new ProjectMapper();
        }

        public async Task<Result<ProjectDto>> CreateAsync(ProjectDto projectDto)
        {
            if (projectDto == null)
                return Result.Fail("Project to create cannot be null.");

            if (string.IsNullOrWhiteSpace(projectDto.Name))
                return Result.Fail("Project name is required.");


            var project = new Project
            {
                Name = projectDto.Name,
                Owner = await _unitOfWork.Users.GetUserByIdAsync(projectDto.OwnerId),
                OwnerId = projectDto.OwnerId
            };

            await _unitOfWork.Projects.CreateAsync(project);
            await _unitOfWork.SaveAsync(); // Mora se sačuvati da bi Project.Id bio dostupan za Board

            // Kreiramo podrazumevani board za projekat
            var boardResult = await _boardService.CreateDefaultBoardAsync(project);
            if (boardResult.IsFailed)
                return Result.Fail(boardResult.Errors);

            project.Board = boardResult.Value;
            project.BoardId = boardResult.Value.Id;
            await _unitOfWork.SaveAsync(); 

            return Result.Ok(MapToProjectDto(project));
        }

        public async Task<Result<ICollection<ProjectDetailsDto>>> GetUserProjects(int userId)
        {
            if (userId <= 0)
                return Result.Fail("Invalid user ID.");

            try
            {
                var projects = await _unitOfWork.Projects.GetUserProjects(userId);

                if (projects == null || projects.Count == 0)
                    return Result.Fail("No projects found for this user.");

                var projectDtos = _projectMapper.CreateDtoList(projects).ToList();

                return Result.Ok((ICollection<ProjectDetailsDto>)projectDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving user projects: {ex.Message}");
            }
        }
        public async Task<Result<ProjectDetailsDto>> GetById(int id)
        {
            if (id <= 0)
                return Result.Fail("Invalid ID.");

            try
            {
                var project = await _unitOfWork.Projects.GetById(id);

                if (project == null)
                    return Result.Fail($"No project found with id: {id}.");


                return Result.Ok(_projectMapper.CreateDto(project));
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving user projects: {ex.Message}");
            }
        }

        private ProjectDto MapToProjectDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                OwnerId = project.OwnerId,
                BoardId = project.BoardId
            };
        }
       
        

    }
}
