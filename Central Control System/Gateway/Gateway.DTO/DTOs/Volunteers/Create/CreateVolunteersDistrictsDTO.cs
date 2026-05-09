namespace Gateway.DTO.DTOs.Volunteers.Create
{
    public class CreateVolunteersDistrictsDTO
    {
        public required Guid VolunteerGID { get; set; }
        public required Guid DistrictGID { get; set; }
    }
}
