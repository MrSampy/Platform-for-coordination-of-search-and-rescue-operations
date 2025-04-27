namespace VolunteerService.Domain.Entities
{
    public class Volunteer : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public int RatingNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public Guid UserGID { get; set; }
    }
}
