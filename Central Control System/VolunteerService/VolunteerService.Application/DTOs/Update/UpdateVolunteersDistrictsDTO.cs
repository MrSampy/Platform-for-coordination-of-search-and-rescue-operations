namespace VolunteerService.Application.DTOs.Update
{
    public class UpdateVolunteersDistrictsDTO
    {
        public required Guid GID { get; set; }
        public required Guid VolunteerGID { get; set; }
        public required Guid DistrictGID { get; set; }
    }
}
