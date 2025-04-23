namespace OperationsService.Application.DTOs
{
    public class GroupDTO : BaseDTO
    {
        public required string Name { get; set; }
        public required Guid EventGID { get; set; }
        public Guid? LeaderGID { get; set; }
    }
}
