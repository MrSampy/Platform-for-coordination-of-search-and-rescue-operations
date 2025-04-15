using AuthService.API.Config;
using AuthService.API.Core.DTO;
using AuthService.API.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserSevice _userSevice;

        public UserController(IUserSevice userSevice)
        {
            _userSevice = userSevice;
        }
        [Authorize]
        [RequiresAuthHeader]
        [HttpGet]
        [Route("collection")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userSevice.GetAllUsers());
        }
        [Authorize]
        [RequiresAuthHeader]
        [HttpGet("bygid/{gid}")]
        public IActionResult GetByGID([FromRoute] Guid gid)
        {
            return Ok(_userSevice.GetByGID(gid));
        }

        [HttpGet("byname/{userName}")]
        public IActionResult GetByUserName([FromRoute] string userName)
        {
            return Ok(_userSevice.GetByUserName(userName));
        }

        [HttpGet("byemail/{email}")]
        public IActionResult GetByEmail([FromRoute] string email)
        {
            return Ok(_userSevice.GetByEmail(email));
        }
        [Authorize]
        [RequiresAuthHeader]
        [HttpGet]
        [Route("{roleName}")]
        public IActionResult GetAllUsers([FromRoute] string roleName)
        {
            return Ok(_userSevice.GetAllUserIdsByRole(roleName));
        }
        [Authorize]
        [RequiresAuthHeader]
        [HttpPut]
        public async Task<IActionResult> UpdateUserRoles(UserDTO query)
        {
            return Ok(await _userSevice.UpdateUserRoles(query));
        }
    }
}
