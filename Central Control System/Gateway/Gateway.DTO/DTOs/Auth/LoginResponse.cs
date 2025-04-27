namespace Gateway.DTO.DTOs.Auth
{
    public class LoginResponse : GetAuthenticatorKeyResponse
    {
        public bool IsValid { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
