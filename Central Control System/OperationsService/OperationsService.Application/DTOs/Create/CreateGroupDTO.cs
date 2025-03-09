namespace OperationsService.Application.DTOs.Create
{
    public class CreateGroupDTO
    {
        public required string Name { get; set; }
        public required Guid EventGID { get; set; }
    }
}
