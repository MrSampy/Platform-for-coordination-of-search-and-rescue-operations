using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Operations.Request
{
    public class GroupPaginationQuery : PaginationQuery
    {
        public Guid? EventGID { get; set; }
        public Guid? LeaderGID { get; set; }
    }
}
