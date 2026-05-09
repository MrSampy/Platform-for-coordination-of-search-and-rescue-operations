using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Operations
{
    public class MessageDTO : BaseDTO
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public Guid EventGID { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsRead { get; set; }
    }
}
