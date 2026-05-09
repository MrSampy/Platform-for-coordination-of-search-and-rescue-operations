namespace OperationsService.Application.DTOs
{
    public class OperationTaskDTO : BaseDTO
    {
        public required string Name { get; set; }
        public required string TaskDescription { get; set; }
        public required Guid GroupGID { get; set; }
        public required Guid TaskStatusGID { get; set; }
    }

}
