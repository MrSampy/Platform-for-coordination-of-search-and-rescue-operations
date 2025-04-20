namespace Gateway.DTO.DTOs.Operations.Request
{
    public class EventStatusChangeRequest
    {
        public required Guid EventGID { get; set; }
        public required Guid EventStatusGID { get; set; }
        public string? Note { get; set; }
    }
}
