namespace Gateway.DTO.DTOs.Volunteers.Request
{
    public class VolunteersForEventRequest
    {
        public required Guid EventGID { get; set; }
        public required bool NotInGroup { get; set; }
    }
}
