namespace Gateway.DTO.DTOs.Auth
{
    public class GetAuthenticatorKeyResponse
    {
        /// <summary>
        /// The key that we have to store it in our Database
        /// </summary>
        public string AuthenticatorKey { get; set; } = string.Empty;
    }
}
