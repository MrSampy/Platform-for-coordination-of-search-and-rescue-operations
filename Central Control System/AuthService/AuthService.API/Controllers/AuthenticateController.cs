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
            return Ok(await _authenticateSevice.RegisterAdmin(model));
        }

        [HttpPost]
        [Route("register-dispatcher")]
        public async Task<IActionResult> RegisterDispatcher([FromBody] RegisterModel model)
        {
            return Ok(await _authenticateSevice.RegisterDispatcher(model));
        }

        [HttpPost]
        [RequiresAuthHeader]
        [Route("register-coordinator")]
        public async Task<IActionResult> RegisterCoordinator([FromBody] RegisterModel model)
        {
            return Ok(await _authenticateSevice.RegisterCoordinator(model));
        }
    }
}
