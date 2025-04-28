namespace Gateway.DTO.DTOs.Volunteers.Create
{
    public class CreateVolunteerDTO
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string SecondName { get; set; }
        public required string Email { get; set; }
        public required string MobilePhone { get; set; }
        public int RatingNumber { get; set; }
        public required DateTime BirthDate { get; set; }
        public required Guid UserGID { get; set; }
    }
}
