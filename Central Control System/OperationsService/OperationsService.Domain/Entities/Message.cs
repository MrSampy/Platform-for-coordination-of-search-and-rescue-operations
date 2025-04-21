namespace OperationsService.Domain.Entities
{
    public class Message : BaseEntity
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public Guid EventGID { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsRead { get; set; }
    }
}
