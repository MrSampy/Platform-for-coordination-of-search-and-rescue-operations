namespace Gateway.DTO.DTOs.Auth
{
    public class MeResponse
    {
        public bool IsValid { get; set; } = false;
        public UserDTO? User { get; set; }
    }
}
