namespace OperationsService.Domain.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; set; }

        public Guid EventGID { get; set; }
    }
}
