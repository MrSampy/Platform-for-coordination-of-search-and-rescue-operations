namespace VolunteerService.Domain.Entities
{
    public class VolunteersGroups : BaseEntity
    {
        public Guid VolunteerGID { get; set; }
        public Guid GroupGID { get; set; }
    }
}
