namespace Gateway.DTO.DTOs.Volunteers.Request
{
    public class UpdateVolunteerRatingRequest
    {
        public required Guid VolunteerGID { get; set; }
        public required int RatingNumber { get; set; }
    }
}
