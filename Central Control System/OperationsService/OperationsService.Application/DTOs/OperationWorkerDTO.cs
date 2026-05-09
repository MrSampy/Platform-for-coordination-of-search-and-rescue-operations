namespace OperationsService.Application.DTOs
{
    public class OperationWorkerDTO : BaseDTO
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string SecondName { get; set; }
        public required string Email { get; set; }
        public required string IdentificationCode { get; set; }
        public required DateTime BirthDate { get; set; }
        public required Guid UserGID { get; set; }
    }
}
