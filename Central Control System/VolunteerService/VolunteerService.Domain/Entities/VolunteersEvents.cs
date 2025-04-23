namespace VolunteerService.Domain.Entities
{
    public class VolunteersEvents : BaseEntity
    {
        public Guid VolunteerGID { get; set; }
        public Guid EventGID { get; set; }
    }
}
