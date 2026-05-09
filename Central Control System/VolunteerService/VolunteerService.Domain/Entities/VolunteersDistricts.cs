namespace VolunteerService.Domain.Entities
{
    public class VolunteersDistricts : BaseEntity
    {
        public Guid VolunteerGID { get; set; }
        public Guid DistrictGID { get; set; }
    }
}
