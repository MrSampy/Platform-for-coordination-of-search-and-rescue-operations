namespace OperationsService.Application.DTOs
{
    public class ErrorModel
    {
        public int StatusCode { get; set; }
        public string message { get; set; }
        public string? Details { get; set; }
    }
}
