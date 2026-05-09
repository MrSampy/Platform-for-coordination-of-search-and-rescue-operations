using System.ComponentModel.DataAnnotations;

namespace OperationsService.Application.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "User Name is required")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
}
