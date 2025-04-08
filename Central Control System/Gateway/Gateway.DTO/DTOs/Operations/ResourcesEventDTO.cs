using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Operations
{
    public class ResourcesEventDTO : BaseDTO
    {
        public required Guid ResourceGID { get; set; }
        public required Guid EventGID { get; set; }
        public required decimal RequiredQuantity { get; set; }
        public required decimal AvailableQuantity { get; set; }
    }
}
