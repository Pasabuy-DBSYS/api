namespace PasabuyAPI.Exceptions
{
    public class InvalidIdFormatException : FormatException
    {
        public InvalidIdFormatException() : base("Invalid Id Format") { }

        public InvalidIdFormatException(string message) : base(message) { }
        public InvalidIdFormatException(string message, Exception inner) : base(message, inner) { }
    }
}