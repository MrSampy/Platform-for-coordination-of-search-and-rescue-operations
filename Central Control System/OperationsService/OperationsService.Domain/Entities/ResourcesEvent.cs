namespace OperationsService.Domain.Entities
{
    public class ResourcesEvent : BaseEntity
    {
        public Guid ResourceGID { get; set; }
        public Guid EventGID { get; set; }
        public decimal RequiredQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
    }
}
