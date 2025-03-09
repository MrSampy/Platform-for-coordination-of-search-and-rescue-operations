namespace OperationsService.Domain.Entities
{
    public class OperationWorker : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SecondName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string IdentificationCode { get; set; }
        public DateTime BirthDate { get; set; }
        public Guid UserGID { get; set; }
    }
}
