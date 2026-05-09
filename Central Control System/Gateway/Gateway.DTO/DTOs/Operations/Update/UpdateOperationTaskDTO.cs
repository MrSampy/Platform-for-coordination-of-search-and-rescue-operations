namespace Gateway.DTO.DTOs.Operations.Update
{
    public class UpdateOperationTaskDTO
    {
        public required Guid GID { get; set; }
        public required string Name { get; set; }
        public required string TaskDescription { get; set; }
        public required Guid GroupGID { get; set; }
        public required Guid TaskStatusGID { get; set; }
    }
}
