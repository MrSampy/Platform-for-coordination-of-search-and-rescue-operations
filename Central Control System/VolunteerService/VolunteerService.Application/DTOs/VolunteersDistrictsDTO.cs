namespace VolunteerService.Application.DTOs
{
    public class VolunteersDistrictsDTO : BaseDTO
    {
        public required Guid VolunteerGID { get; set; }
        public required Guid DistrictGID { get; set; }
    }
}
