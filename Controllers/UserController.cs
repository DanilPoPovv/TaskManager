using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Requests;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var userView = await _userService.Register(request);
            return Ok(userView);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var authView = await _userService.Login(request);
            return Ok(authView);
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteUserRequest request)
        {
            var result = await _userService.DeleteUser(request);
            return Ok(result);
        }
        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "User,Admin")]
        public async Task<UserView> Update(UserUpdateRequest request)
        {
            var user = await _userService.Update(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!, request);
            return new UserView(user);
        }
    }
}
