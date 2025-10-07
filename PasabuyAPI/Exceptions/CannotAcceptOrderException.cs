namespace PasabuyAPI.Exceptions
{
    public class CannotAcceptOrderException : Exception
    {
        public CannotAcceptOrderException() : base("Cannot accept order") { }

        public CannotAcceptOrderException(string message) : base(message) { }
        public CannotAcceptOrderException(string message, Exception inner) : base(message, inner) { }
    }
}