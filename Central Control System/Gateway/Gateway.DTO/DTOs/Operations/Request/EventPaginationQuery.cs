using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Operations.Request
{
    public class EventPaginationQuery : PaginationQuery
    {
        public Guid? EventStatusGID { get; set; }
        public Guid? DispatcherGID { get; set; }
        public Guid? CoordinatorGID { get; set; }
        public Guid? EventTypeGID { get; set; }

        public override string GetKey()
        {
            return $"{base.GetKey()}EventStatusGID:{EventStatusGID};DispatcherGID:{DispatcherGID};CoordinatorGID:{CoordinatorGID};EventTypeGID:{EventTypeGID}";
        }
    }
}
