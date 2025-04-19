using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Operations.Request
{
    public class EventPaginationQuery : PaginationQuery
    {
        public Guid? EventStatusGID { get; set; }
    }
}
