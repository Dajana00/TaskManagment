using System.Configuration;
using Trello.DTOs;
using Trello.Model;

namespace Trello.Validation
{
    public class CreateCardValidator : Validator<CreateCardDto>
    {
        public CreateCardValidator()
        {
            Add("NotNull", new IsNotNullSpecification<CreateCardDto>("Card to create cannot be null."));
            Add("NameRequired", new PropertyNotEmptySpecification<CreateCardDto>(x => x.Title, "Card title is required."));
            Add("NameRequired", new PropertyNotEmptySpecification<CreateCardDto>(x => x.Description, "Card description is required."));
        }
    }
}
