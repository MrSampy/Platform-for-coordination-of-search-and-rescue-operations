using AuthService.API.Config;
using AuthService.API.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Authorize]
    [ApiController]
    [RequiresAuthHeader]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleSevice _roleSevice;

        public RoleController(IRoleSevice roleSevice)
        {
            _roleSevice = roleSevice;
        }


        [HttpGet]
        [Route("collection")]
        public IActionResult GetAllRoles()
        {
            return Ok(_roleSevice.GetAllRoles());
        }
    }
}
