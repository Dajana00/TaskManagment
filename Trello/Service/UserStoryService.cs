using AutoMapper;
using FluentResults;
using Trello.DTOs;
using Trello.Mapper;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Service.IService;

namespace Trello.Service
{
    public class UserStoryService : IUserStoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UserStoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<Result<UserStoryDto>> CreateAsync(UserStoryDto userStoryDto)
        {
            if (userStoryDto == null)
                return Result.Fail("UserStory to create cannot be null.");

            if (string.IsNullOrWhiteSpace(userStoryDto.Title))
                return Result.Fail("UserStory title is required.");

            if (string.IsNullOrWhiteSpace(userStoryDto.Description))
                return Result.Fail("UserStory description is required.");


            var userStory = new UserStory
            {
                Title = userStoryDto.Title,
                Description = userStoryDto.Description,
                BacklogId = userStoryDto.BacklogId
            };

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

    }
}
