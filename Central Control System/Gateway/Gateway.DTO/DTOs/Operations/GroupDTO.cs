using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Operations
{
    public class GroupDTO : BaseDTO
    {
        public required string Name { get; set; }
        public required Guid EventGID { get; set; }
        public Guid? LeaderGID { get; set; }
    }
}
