namespace PasabuyAPI.Exceptions
{
    public class CannotCreateOrderException : Exception
    {
        public CannotCreateOrderException() : base("Cannot create order") { }

        public CannotCreateOrderException(string message) : base(message) { }
        public CannotCreateOrderException(string message, Exception inner) : base(message, inner) { }
    }
}