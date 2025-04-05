using AuthService.API.Config;
using AuthService.API.Core.Interfaces;
using AuthService.API.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateSevice _authenticateSevice;

        public AuthenticateController(IAuthenticateSevice authenticateSevice)
        {
            _authenticateSevice = authenticateSevice;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return Ok(await _authenticateSevice.Login(model));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            return Ok(await _authenticateSevice.Register(model));
        }

        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            await _authenticateSevice.RegisterAdmin(model);

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-dispatcher")]
        public async Task<IActionResult> RegisterDispatcher([FromBody] RegisterModel model)
        {
            await _authenticateSevice.RegisterDispatcher(model);

            return Ok(new Response { Status = "Success", Message = "Dispatcher created successfully!" });
        }

        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-coordinator")]
        public async Task<IActionResult> RegisterCoordinator([FromBody] RegisterModel model)
        {
            await _authenticateSevice.RegisterCoordinator(model);

            return Ok(new Response { Status = "Success", Message = "Coordinator created successfully!" });
        }
    }
}
