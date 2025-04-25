namespace Trello.Validation
{
    public abstract class Validator<T>   // ovo je kao menazder za sve validacije entitea
    {
        private readonly List<Rule<T>> _rules = new();

        protected void Add(string key, ISpecification<T> specification)
        {
            _rules.Add(new Rule<T>(specification, key));
        }

        public List<string> Validate(T entity)
        {
            return _rules
                .Where(r => !r.Specification.IsSatisfiedBy(entity))
                .Select(r => r.Specification.ErrorMessage)
                .ToList();
        }
    }

}
