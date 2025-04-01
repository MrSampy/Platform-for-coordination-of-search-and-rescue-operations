namespace OperationsService.Application.DTOs.Update
{
    public class UpdateGroupDTO
    {
        public required Guid GID { get; set; }
        public required string Name { get; set; }
        public required Guid EventGID { get; set; }
        public Guid LeaderGID { get; set; }
    }
}
