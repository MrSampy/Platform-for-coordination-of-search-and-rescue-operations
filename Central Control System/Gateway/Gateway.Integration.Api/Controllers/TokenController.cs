using Gateway.Application.Events;
using Gateway.Application.Filters;
using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Auth;
using Gateway.DTO.Exceptions;
using Gateway.Integration.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Gateway.Integration.Api.Controllers
{
    [ApiController]
    [Route("gateway.integration.api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    public class TokenController : ControllerBase
    {
        private readonly IAuthGateway _authGateway;
        private readonly IRabbitMqPublisher _publisher;

        public TokenController(IAuthGateway authGateway, IRabbitMqPublisher publisher)
        {
            _authGateway = authGateway;
            _publisher = publisher;
        }

        [HttpPost]
        [Route("login")]
        [SwaggerRequestExample(typeof(LoginModel), typeof(LoginModelExample))]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var evt = new SendMessageEvent { Message = $"{model.Username + model.Password}" };
            _publisher.Publish("token.send", evt);

            var loginResponse = await _authGateway.Login(model);

            if (loginResponse.IsValid)
            {
                var tokenInfo = await _authGateway.GetToken(new GetTokenRequest
                {
                    Username = model.Username
                });

                return Ok(tokenInfo);
            }
            else
            {
                throw new ServiceException(loginResponse.Message);
            }
        }

    }
}
