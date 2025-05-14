using Trello.DTOs;

namespace Trello.Validation.SprintValidator
{
    public class EndDateBeginingSpecification : ISpecification<SprintDto>
    {
        public string ErrorMessage => "End date cannot be before start date.";

        public bool IsSatisfiedBy(SprintDto entity)
            => entity.EndDate > entity.StartDate;
    }

}
