namespace VolunteerService.Application.DTOs.Create
{
    public class CreateVolunteersDistrictsDTO
    {
        public required Guid VolunteerGID { get; set; }
        public required Guid DistrictGID { get; set; }
    }
}
