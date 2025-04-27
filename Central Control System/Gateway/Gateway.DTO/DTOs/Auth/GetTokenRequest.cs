namespace Gateway.DTO.DTOs.Auth
{
    public class GetTokenRequest
    {

        /// <summary>
        /// Generated key the user must insert it in his authenticator app
        /// You can use any QRCode library the facilate the reading of this key instead of typing it.
        /// Try the QRCode js library
        /// </summary>
        public string Code { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
