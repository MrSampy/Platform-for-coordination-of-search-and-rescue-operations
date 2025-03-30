namespace OperationsService.Domain.Entities
{
    public class Event : BaseEntity
    {
        public string Name { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public Guid EventTypeGID { get; set; }
        public Guid DistrictGID { get; set; }
        public Guid CoordinatorGID { get; set; }
        public Guid DispatcherGID { get; set; }
        public Guid EventStatusGID { get; set; }
    }
}
