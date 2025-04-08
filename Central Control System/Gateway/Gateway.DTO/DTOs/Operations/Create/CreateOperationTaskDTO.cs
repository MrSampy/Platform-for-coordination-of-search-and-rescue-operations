namespace Gateway.DTO.DTOs.Operations.Create
{
    public class CreateOperationTaskDTO
    {
        public required string Name { get; set; }
        public required string TaskDescription { get; set; }
        public required Guid GroupGID { get; set; }
        public required Guid TaskStatusGID { get; set; }
    }
}
