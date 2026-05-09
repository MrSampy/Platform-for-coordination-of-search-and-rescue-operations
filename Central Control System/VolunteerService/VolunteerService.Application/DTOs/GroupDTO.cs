namespace VolunteerService.Application.DTOs
{
    internal class GroupDTO
    {
        public Guid GID { get; set; }
        public string Name { get; set; }
        public Guid EventGID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
