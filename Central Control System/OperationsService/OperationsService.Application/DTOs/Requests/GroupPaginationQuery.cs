using OperationsService.Domain.Entities;

namespace OperationsService.Application.DTOs.Requests
{
    public class GroupPaginationQuery : PaginationQuery
    {
        public Guid? EventGID { get; set; }
        public Guid? LeaderGID { get; set; }
    }
}
