namespace Gateway.DTO.DTOs.Operations.Request
{
    public class CreateEventRequest
    {
        public required string Name { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public required Guid EventTypeGID { get; set; }
        public required Guid DistrictGID { get; set; }
        public required Guid CoordinatorGID { get; set; }
        public required Guid UserGID { get; set; }
    }
}
