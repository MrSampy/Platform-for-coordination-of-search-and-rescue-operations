namespace AuthService.API.Core.Models
{
    public class LoginResponse : GetAuthenticatorKeyResponse
    {
        public bool IsValid { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
