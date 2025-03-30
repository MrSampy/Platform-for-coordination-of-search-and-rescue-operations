namespace OperationsService.Application.DTOs
{
    public class EventDTO : BaseDTO
    {
        public required string Name { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public required Guid EventTypeGID { get; set; }
        public required Guid DistrictGID { get; set; }
        public required Guid CoordinatorGID { get; set; }
        public required Guid DispatcherGID { get; set; }
        public required Guid EventStatusGID { get; set; }
    }
}
