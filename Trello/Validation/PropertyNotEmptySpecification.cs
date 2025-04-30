namespace Trello.Validation
{
    public class PropertyNotEmptySpecification<T> : ISpecification<T>
    {
        private readonly Func<T, string> _selector;
        public string ErrorMessage { get; }

        public PropertyNotEmptySpecification(Func<T, string> selector, string errorMessage)
        {
            _selector = selector;
            ErrorMessage = errorMessage;
        }

        public bool IsSatisfiedBy(T entity) => !string.IsNullOrWhiteSpace(_selector(entity));
    }

}
