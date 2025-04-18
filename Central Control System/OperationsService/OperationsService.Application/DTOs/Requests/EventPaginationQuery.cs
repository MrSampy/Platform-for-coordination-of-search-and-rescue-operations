using OperationsService.Domain.Entities;

namespace OperationsService.Application.DTOs.Requests
{
    public class EventPaginationQuery : PaginationQuery
    {
        public Guid? EventStatusGID { get; set; }
    }
}
