using OperationsService.Domain.Entities;

namespace OperationsService.Application.DTOs.Requests
{
    public class MessagePaginationQuery : PaginationQuery
    {
        public bool? IsRead { get; set; }
        public Guid? From { get; set; }
        public Guid? To { get; set; }
        public Guid? EventGID { get; set; }
    }
}
