namespace OperationsService.Application.DTOs.Responses
{
    public class LoginResponse
    {
        public bool IsValid { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
