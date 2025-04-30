using Trello.DTOs;

namespace Trello.Validation
{
    public class StartDateNotInPastSpecification : ISpecification<SprintDto>
    {
        public string ErrorMessage => "Sprint cannot start in the past.";

        public bool IsSatisfiedBy(SprintDto entity)
            => entity.StartDate >= DateTime.UtcNow.Date;
    }

}
