namespace VolunteerService.Application.DTOs
{
    public class VolunteersEventsDTO : BaseDTO
    {
        public required Guid VolunteerGID { get; set; }
        public required Guid EventGID { get; set; }
    }
}
