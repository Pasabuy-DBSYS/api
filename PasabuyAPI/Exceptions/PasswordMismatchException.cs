namespace PasabuyAPI.Exceptions
{
    public class PasswordMismatchException : Exception
    {
        public PasswordMismatchException() : base("Password does not match") { }

        public PasswordMismatchException(string message) : base(message) { }
        public PasswordMismatchException(string message, Exception inner) : base(message, inner) { }
    }
}