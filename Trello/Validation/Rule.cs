namespace Trello.Validation
{
    public class Rule<T>
    {
        public ISpecification<T> Specification { get; }
        public string Key { get; }

        public Rule(ISpecification<T> specification, string key)
        {
            Specification = specification;
            Key = key;
        }
    }

}
