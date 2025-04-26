using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Operations.Request
{
    public class GetGroupsByDispatcherGIDRequest : PaginationQuery
    {
        public required Guid DispatcherGID { get; set; }
    }
}
