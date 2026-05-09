using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Volunteers
{
    public class VolunteersGroupsDTO : BaseDTO
    {
        public required Guid VolunteerGID { get; set; }
        public required Guid GroupGID { get; set; }
    }
}
