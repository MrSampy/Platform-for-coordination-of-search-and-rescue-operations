using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Operations
{
    public class OperationTaskDTO : BaseDTO
    {
        public required string Name { get; set; }
        public required string TaskDescription { get; set; }
        public required Guid GroupGID { get; set; }
        public required Guid TaskStatusGID { get; set; }
    }

}
