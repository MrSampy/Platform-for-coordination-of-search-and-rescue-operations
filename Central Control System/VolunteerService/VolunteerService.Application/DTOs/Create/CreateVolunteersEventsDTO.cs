namespace VolunteerService.Application.DTOs.Create
{
    public class CreateVolunteersEventsDTO
    {
        public required Guid VolunteerGID { get; set; }
        public required Guid EventGID { get; set; }
    }
}
