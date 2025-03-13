namespace VolunteerService.Application.DTOs
{
    public class VolunteersGroupsDTO : BaseDTO
    {
        public required Guid VolunteerGID { get; set; }
        public required Guid GroupGID { get; set; }
    }
}
