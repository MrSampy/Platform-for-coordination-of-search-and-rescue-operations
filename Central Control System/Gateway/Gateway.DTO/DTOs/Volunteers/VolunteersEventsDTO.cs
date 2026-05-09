using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Volunteers
{
    public class VolunteersEventsDTO : BaseDTO
    {
        public required Guid VolunteerGID { get; set; }
        public required Guid EventGID { get; set; }
    }
}
