namespace UtilsService.Application.DTOs.Response
{
    public class LoginResponse
    {
        public bool IsValid { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
