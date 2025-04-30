using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Trello.DTOs;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;

namespace Trello.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBoardService _boardService;   
        private readonly IBacklogService _backlogService;
        private readonly IMapper _projectMapper;  
        private readonly IUserStoryService _userStoryService;   
        

        public ProjectService(IUnitOfWork unitOfWork, IBoardService boardService,
            IBacklogService backlogService,IMapper mapper,
            IUserStoryService userStoryService)
        {
            _unitOfWork = unitOfWork;
            _boardService = boardService; 
            _backlogService = backlogService;
            _projectMapper = mapper;
            _userStoryService = userStoryService;   
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
            await _unitOfWork.SaveAsync();

            
            var boardResult = await _boardService.CreateDefaultBoardAsync(project);
            if (boardResult.IsFailed)
                return Result.Fail(boardResult.Errors);

            project.Board = boardResult.Value;
            project.BoardId = boardResult.Value.Id;

            var backlogResult = await _backlogService.CreateDefaultBacklogAsync(project);
            if (backlogResult.IsFailed)
                return Result.Fail(backlogResult.Errors);

            project.Backlog = backlogResult.Value;
            project.BacklogId = backlogResult.Value.Id;

            await _unitOfWork.SaveAsync(); 

            return Result.Ok(_projectMapper.Map<ProjectDto>(project));
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
                var projectDtos = new List<ProjectDetailsDto>();        
                foreach (var project in projects)
                {
                    projectDtos.Add(_projectMapper.Map<ProjectDetailsDto>(project));
                }

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


                return Result.Ok(_projectMapper.Map<ProjectDetailsDto>(project));
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving user projects: {ex.Message}");
            }
        }

        public async Task<Result<ProjectDetailsDto>> GetByUserStory(int userStoryId)
        {
            try
            {
                var userStoryDto = await _userStoryService.GetByIdAsync(userStoryId);
                if (userStoryDto == null)
                    return Result.Fail($"Error fetching project by user story. User story with id {userStoryId} not found.");

                var backlogResult = await _backlogService.GetById(userStoryDto.Value.BacklogId);
                if (backlogResult == null)
                    return Result.Fail($"Error fetching project by user story.Backlog not found.");

                var projectResult = await GetById(backlogResult.Value.ProjectId);
                if (projectResult == null)
                    return Result.Fail("Project not found.");

                return Result.Ok(projectResult.Value);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving project by user story id: {ex.Message}");
            }
        }
    }
}
