using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Operations.Request
{
    public class MessagePaginationQuery : PaginationQuery
    {
        public bool? IsRead { get; set; }
        public Guid? From { get; set; }
        public Guid? To { get; set; }
        public Guid? EventGID { get; set; }
        public override string GetKey()
        {
            return $"{base.GetKey()}IsRead:{IsRead};From:{From};To:{To};EventGID:{EventGID}";
        }
    }
}
