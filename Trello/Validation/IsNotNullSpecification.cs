namespace Trello.Validation
{
    public class IsNotNullSpecification<T> : ISpecification<T>
    {
        public string ErrorMessage { get; }

        public IsNotNullSpecification(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool IsSatisfiedBy(T entity) => entity != null;
    }

}
