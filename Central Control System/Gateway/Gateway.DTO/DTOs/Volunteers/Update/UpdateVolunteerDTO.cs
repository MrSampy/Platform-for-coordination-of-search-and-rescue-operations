namespace Gateway.DTO.DTOs.Volunteers.Update
{
    public class UpdateVolunteerDTO
    {
        public required Guid GID { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string SecondName { get; set; }
        public required string Email { get; set; }
        public required string MobilePhone { get; set; }
        public required DateTime BirthDate { get; set; }
        public required Guid UserGID { get; set; }
    }

}
