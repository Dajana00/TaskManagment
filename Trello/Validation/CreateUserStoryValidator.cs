using Trello.DTOs;

namespace Trello.Validation
{
    public class CreateUserStoryValidator : Validator<UserStoryDto>
    {
        public CreateUserStoryValidator()
        {
            Add("NotNull", new IsNotNullSpecification<UserStoryDto>("User story to create cannot be null."));
            Add("NameRequired", new PropertyNotEmptySpecification<UserStoryDto>(x => x.Title, "User story title is required."));
            Add("NameRequired", new PropertyNotEmptySpecification<UserStoryDto>(x => x.Description, "User story description is required."));
        }
    }
}
