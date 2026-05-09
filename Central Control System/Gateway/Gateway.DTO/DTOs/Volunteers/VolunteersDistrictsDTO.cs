using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Volunteers
{
    public class VolunteersDistrictsDTO : BaseDTO
    {
        public required Guid VolunteerGID { get; set; }
        public required Guid DistrictGID { get; set; }
    }
}
