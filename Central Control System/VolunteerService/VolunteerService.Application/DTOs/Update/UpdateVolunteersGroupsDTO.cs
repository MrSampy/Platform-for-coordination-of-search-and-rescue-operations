namespace VolunteerService.Application.DTOs.Update
{
    public class UpdateVolunteersGroupsDTO
    {
        public required Guid GID { get; set; }
        public required Guid VolunteerGID { get; set; }
        public required Guid GroupGID { get; set; }
    }
}
