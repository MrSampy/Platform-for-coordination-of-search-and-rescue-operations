namespace OperationsService.Application.DTOs.Create
{
    public class CreateMessageDTO
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public Guid EventGID { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsRead { get; set; }
    }
}
