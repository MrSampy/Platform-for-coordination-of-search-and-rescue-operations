namespace AuthService.API.Core.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required List<RoleDTO> Roles { get; set; }
    }
}
