using Gateway.DTO.DTOs.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace Gateway.Application.Filters
{
    public class LoginModelExample : IExamplesProvider<LoginModel>
    {
        public LoginModel GetExamples()
        {
            return new LoginModel
            {
                Username = "admin1",
                Password = "*Password1Password"
            };
        }
    }
}
