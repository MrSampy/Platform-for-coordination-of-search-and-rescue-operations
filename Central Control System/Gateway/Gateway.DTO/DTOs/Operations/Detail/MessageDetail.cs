namespace Gateway.DTO.DTOs.Operations.Detail
{
    public class MessageDetail : MessageDTO
    {
        public string Sender { get; set; } = string.Empty;
        public string Receiver { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
    }
}
