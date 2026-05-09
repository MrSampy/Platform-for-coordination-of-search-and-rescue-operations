namespace Gateway.DTO.DTOs.Operations.Detail
{
    public class MessageDetails : MessageDTO
    {
        public string Sender { get; set; } = string.Empty;
        public string Receiver { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
    }
}
