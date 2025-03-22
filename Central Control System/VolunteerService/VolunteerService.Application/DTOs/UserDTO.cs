namespace VolunteerService.Application.DTOs
{
    internal class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<RoleDTO> Roles { get; set; }
    }
}
