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
        private readonly UserStoryMapper _userStoryMapper;


        public UserStoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userStoryMapper = new UserStoryMapper();   

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

            return Result.Ok(_userStoryMapper.CreateDto(userStory));
        }
    }
}
