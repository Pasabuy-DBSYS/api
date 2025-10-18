namespace PasabuyAPI.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() : base("Attribute already exists") { }

        public AlreadyExistsException(string message) : base(message) { }
        public AlreadyExistsException(string message, Exception inner) : base(message, inner) { }
    } 
}