using AutoMapper;
using FluentResults;
using Trello.DTOs;
using Trello.Mapper;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;
using Trello.Validation.UserStoryValidator;

namespace Trello.Service
{
    public class UserStoryService : IUserStoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CreateUserStoryValidator _validator;
        


        public UserStoryService(IUnitOfWork unitOfWork, IMapper mapper, CreateUserStoryValidator createUserStoryValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = createUserStoryValidator;  

        }

        public async Task<Result<UserStoryDto>> CreateAsync(UserStoryDto userStoryDto)
        {
            _validator.Validate(userStoryDto);
            var userStory  = _mapper.Map<UserStory>(userStoryDto);  
            
            await _unitOfWork.UserStories.CreateAsync(userStory);
            await _unitOfWork.SaveAsync();

            return Result.Ok(_mapper.Map<UserStoryDto>(userStory));
        }

        public async Task<Result<ICollection<UserStoryDto>>> GetAll()
        {
            try
            {
                var userStories = await _unitOfWork.UserStories.GetAll();

                if (userStories == null || userStories.Count == 0)
                    return Result.Fail("No user stories found.");

                var userStoryDtos = userStories
                    .Select(us => _mapper.Map<UserStoryDto>(us))
                    .ToList();

                return Result.Ok((ICollection<UserStoryDto>)userStoryDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving user stories: {ex.Message}");
            }
        }


        public async Task<Result<ICollection<UserStoryDto>>> GetByBacklogIdAsync(int backlogId)
        {
            if (backlogId <= 0)
                return Result.Fail("Invalid backlog ID.");

            try
            {
                var userStories = await _unitOfWork.UserStories.GetByBacklogId(backlogId);

                if (userStories == null || userStories.Count == 0)
                    return Result.Fail("No user stories found for the provided backlog ID.");

                var userStoryDtos = userStories
                    .Select(us => _mapper.Map<UserStoryDto>(us))
                    .ToList();

                return Result.Ok((ICollection<UserStoryDto>)userStoryDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving user stories: {ex.Message}");
            }
        }
        public async Task<Result<UserStoryDto>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return Result.Fail("Invalid ID.");

            try
            {
                var userStory = await _unitOfWork.UserStories.GetByIdAsync(id);

                if (userStory == null)
                    return Result.Fail($"No user story found with id: {id}.");


                return Result.Ok(_mapper.Map<UserStoryDto>(userStory));
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while retrieving user story with id {id}: {ex.Message}");
            }
        }


    }
}
