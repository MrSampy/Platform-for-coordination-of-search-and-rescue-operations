namespace Gateway.DTO.DTOs.Auth
{
    public class TokenInfoDTO
    {
        public required string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
