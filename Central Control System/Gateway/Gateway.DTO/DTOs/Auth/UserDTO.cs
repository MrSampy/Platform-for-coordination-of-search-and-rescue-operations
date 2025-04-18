namespace Gateway.DTO.DTOs.Auth
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<RoleDTO> Roles { get; set; }
    }
}
