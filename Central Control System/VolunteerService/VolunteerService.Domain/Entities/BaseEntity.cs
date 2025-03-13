namespace VolunteerService.Domain.Entities
{
    public class BaseEntity
    {
        public Guid GID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
