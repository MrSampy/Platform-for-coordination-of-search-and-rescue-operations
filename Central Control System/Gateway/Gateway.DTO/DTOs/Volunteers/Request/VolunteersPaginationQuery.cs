using Gateway.DTO.DTOs.Common;

namespace Gateway.DTO.DTOs.Volunteers.Request
{
    public class VolunteersPaginationQuery : PaginationQuery
    {
        public string? SortBy { get; set; }
        public bool? IsDescending { get; set; } = false;
    }
}
