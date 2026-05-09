namespace OperationsService.Application.DTOs.Update
{
    public class UpdateResourcesEventDTO
    {
        public required Guid GID { get; set; }
        public required Guid ResourceGID { get; set; }
        public required Guid EventGID { get; set; }
        public required Guid MeasurementUnitGID { get; set; }
        public required decimal RequiredQuantity { get; set; }
        public required decimal AvailableQuantity { get; set; }
    }
}
