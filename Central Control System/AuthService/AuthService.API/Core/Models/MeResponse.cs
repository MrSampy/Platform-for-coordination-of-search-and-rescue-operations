using AuthService.API.Core.DTO;

namespace AuthService.API.Core.Models
{
    public class MeResponse
    {
        public bool IsValid { get; set; } = false;
        public UserDTO? User { get; set; }
    }
}
