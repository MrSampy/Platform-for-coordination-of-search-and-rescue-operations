namespace OperationsService.Application.DTOs.Create
{
    public class CreateResourcesEventDTO
    {
        public required Guid ResourceGID { get; set; }
        public required Guid EventGID { get; set; }
        public required decimal RequiredQuantity { get; set; }
        public required decimal AvailableQuantity { get; set; }
    }
}
