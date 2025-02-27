namespace AuthService.API.Exceptions
{
    public class AuthServiceException : Exception
    {
        public AuthServiceException(string message) : base(message)
        {
        }
    }
}
