namespace OperationsService.Domain.Entities
{
    public class OperationTask : BaseEntity
    {
        public string Name { get; set; }
        public string TaskDescription { get; set; }
        public Guid GroupGID { get; set; }
        public Guid TaskStatusGID { get; set; }
    }
}
