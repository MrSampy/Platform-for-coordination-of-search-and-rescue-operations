namespace Gateway.DTO.DTOs.Volunteers.Create
{
    public class CreateVolunteersEventsDTO
    {
        public required Guid VolunteerGID { get; set; }
        public required Guid EventGID { get; set; }
    }
}
