namespace VolunteerService.Application.DTOs.Create
{
    public class CreateVolunteersGroupsDTO
    {
        public required Guid VolunteerGID { get; set; }
        public required Guid GroupGID { get; set; }
    }
}
