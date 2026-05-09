namespace Gateway.DTO.DTOs.Operations.Detail
{
    public class EventDetails : EventDTO
    {
        public required string EventType { get; set; }
        public required string District { get; set; }
        public required string Coordinator { get; set; }
        public required string Dispatcher { get; set; }
        public required string EventStatus { get; set; }
    }
}
