namespace Gateway.DTO.DTOs.Auth
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required List<RoleDTO> Roles { get; set; }
    }
}
