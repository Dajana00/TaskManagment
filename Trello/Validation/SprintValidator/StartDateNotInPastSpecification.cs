using Trello.DTOs;

namespace Trello.Validation.SprintValidator
{
    public class StartDateNotInPastSpecification : ISpecification<SprintDto>
    {
        public string ErrorMessage => "Sprint cannot start in the past.";

        public bool IsSatisfiedBy(SprintDto entity)
            => entity.StartDate >= DateTime.UtcNow.Date;
    }

}
