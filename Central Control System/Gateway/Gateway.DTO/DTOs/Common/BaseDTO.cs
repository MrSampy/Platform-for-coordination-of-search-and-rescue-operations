namespace Gateway.DTO.DTOs.Common
{
    public class BaseDTO
    {
        public required Guid GID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
