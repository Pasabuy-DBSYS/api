namespace PasabuyAPI.Exceptions
{
    public class EmptyFieldException : Exception
    {
        public EmptyFieldException() : base("Field is empty") { }

        public EmptyFieldException(string message) : base(message) { }
        public EmptyFieldException(string message, Exception inner) : base(message, inner) { }
    }
}