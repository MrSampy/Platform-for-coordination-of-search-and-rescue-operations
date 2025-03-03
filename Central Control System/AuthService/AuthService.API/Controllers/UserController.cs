using AuthService.API.Config;
using AuthService.API.Core.DTO;
using AuthService.API.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Authorize]
    [ApiController]
    [RequiresAuthHeader]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserSevice _userSevice;

        public UserController(IUserSevice userSevice)
        {
            _userSevice = userSevice;
        }


        [HttpGet]
        [Route("collection")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userSevice.GetAllUsers());
        }

        [HttpGet]
        [Route("{roleName}")]
        public IActionResult GetAllUsers([FromRoute] string roleName)
        {
            return Ok(_userSevice.GetAllUserIdsByRole(roleName));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserRoles(UserDTO query)
        {
            return Ok(await _userSevice.UpdateUserRoles(query));
        }
    }
}
