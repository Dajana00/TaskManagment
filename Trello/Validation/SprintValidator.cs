using Trello.DTOs;
using Trello.Model;

namespace Trello.Validation
{
    public class SprintValidator : Validator<SprintDto>
    {
        public SprintValidator(IEnumerable<Sprint> existingSprints)
        {
            Add("NotNull", new IsNotNullSpecification<SprintDto>("Sprint to create cannot be null."));
            Add("NameRequired", new PropertyNotEmptySpecification<SprintDto>(x => x.Name, "Sprint name is required."));
            Add("StartDateValid", new StartDateNotInPastSpecification());
            Add("EndDateAfterStart", new EndDateBeginingSpecification());
            Add("NoOverlap", new OverlappingSprintSpecification(existingSprints));
        }
    }

}
