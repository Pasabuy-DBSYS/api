namespace PasabuyAPI.Exceptions
{
    public class InvalidEmailFormatException : FormatException
    {
        public InvalidEmailFormatException() : base("Invalid Email Format") { }

        public InvalidEmailFormatException(string message) : base(message) { }
        public InvalidEmailFormatException(string message, Exception inner) : base(message, inner) { }
    }
}