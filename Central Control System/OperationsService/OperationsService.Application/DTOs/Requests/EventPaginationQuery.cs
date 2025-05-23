using OperationsService.Domain.Entities;

namespace OperationsService.Application.DTOs.Requests
{
    public class EventPaginationQuery : PaginationQuery
    {
        public Guid? EventStatusGID { get; set; }
        public Guid? DispatcherGID { get; set; }
        public Guid? CoordinatorGID { get; set; }
        public Guid? EventTypeGID { get; set; }
        public bool SortByCreateDate { get; set; } = true;
    }
}
