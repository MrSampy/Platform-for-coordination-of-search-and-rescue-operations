using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Operations.Clear
{
    public class DetailEvent : BaseDTO
    {
        public required string Name { get; set; }
        public required decimal Longitude { get; set; }
        public required decimal Latitude { get; set; }
        public required string EventType { get; set; }
        public required string District { get; set; }
        public required string Coordinator { get; set; }
        public required string Dispatcher { get; set; }
        public required string EventStatus { get; set; }
    }
}
