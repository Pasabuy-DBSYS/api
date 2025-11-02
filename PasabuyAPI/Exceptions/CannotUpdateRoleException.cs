namespace PasabuyAPI.Exceptions
{
    public class CannotUpdateRoleException : Exception
    {
        public CannotUpdateRoleException() : base("Cannot switch roles.") { }

        public CannotUpdateRoleException(string message) : base(message) { }
        public CannotUpdateRoleException(string message, Exception inner) : base(message, inner) { }
    }
}