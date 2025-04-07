using Trello.DTOs;
using Trello.Model;

namespace Trello.Mapper
{
    public class UserStoryMapper
    {
        public UserStoryMapper()
        {

        }

        public UserStoryDto CreateDto(UserStory userStory) 
        {
            return new UserStoryDto 
            {
                Id = userStory.Id,
                Title = userStory.Title,
                Description = userStory.Description,
                BacklogId = userStory.BacklogId,    
            };
        }

        public ICollection<UserStoryDto> CreateDtoList(ICollection<UserStory> userStories)
        {
            return userStories.Select(CreateDto).ToList();
        }
    }
}
