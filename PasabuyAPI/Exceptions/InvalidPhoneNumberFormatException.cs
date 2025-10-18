namespace PasabuyAPI.Exceptions
{
    public class InvalidPhoneNumberFormatException : FormatException
    {
        public InvalidPhoneNumberFormatException() : base("Invalid Phone Format") { }

        public InvalidPhoneNumberFormatException(string message) : base(message) { }
        public InvalidPhoneNumberFormatException(string message, Exception inner) : base(message, inner) { }
    }
}