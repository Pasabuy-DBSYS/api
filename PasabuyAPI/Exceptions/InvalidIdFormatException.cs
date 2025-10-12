namespace PasabuyAPI.Exceptions
{
    public class InvalidIdFormatException : Exception
    {
        public InvalidIdFormatException() : base("Invalid Id Format") { }

        public InvalidIdFormatException(string message) : base(message) { }
        public InvalidIdFormatException(string message, Exception inner) : base(message, inner) { }
    }
}