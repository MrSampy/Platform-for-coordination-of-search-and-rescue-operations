namespace Gateway.DTO.DTOs.Volunteers.Create
{
    public class CreateVolunteersGroupsDTO
    {
        public required Guid VolunteerGID { get; set; }
        public required Guid GroupGID { get; set; }
    }
}
