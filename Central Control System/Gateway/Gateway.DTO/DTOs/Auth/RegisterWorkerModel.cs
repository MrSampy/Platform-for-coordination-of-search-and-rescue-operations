using System.ComponentModel.DataAnnotations;

namespace Gateway.DTO.DTOs.Auth
{
    public class RegisterWorkerModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public required string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        public required string Surname { get; set; }
        [Required(ErrorMessage = "SecondName is required")]
        public required string SecondName { get; set; }
        [Required(ErrorMessage = "IdentificationCode is required")]
        public required string IdentificationCode { get; set; }
        [Required(ErrorMessage = "BirthDate is required")]
        public required DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public required string Role { get; set; }
    }
}
