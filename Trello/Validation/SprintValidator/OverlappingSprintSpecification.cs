using Trello.DTOs;
using Trello.Model;

namespace Trello.Validation.SprintValidator
{
    public class OverlappingSprintSpecification : ISpecification<SprintDto>
    {
        private readonly IEnumerable<Sprint> _existingSprints;

        public OverlappingSprintSpecification(IEnumerable<Sprint> existingSprints)
        {
            _existingSprints = existingSprints;
        }

        public string ErrorMessage => "Sprint overlaps with an existing sprint.";

        public bool IsSatisfiedBy(SprintDto entity)
        {
            return !_existingSprints.Any(existing =>
                entity.StartDate < existing.EndDate &&
                entity.EndDate > existing.StartDate);
        }
    }

}
