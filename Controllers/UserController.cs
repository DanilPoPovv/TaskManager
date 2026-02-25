using Microsoft.AspNetCore.Mvc;
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
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var userView = await _userService.Register(request);
            return Ok(userView);
        }
    }
}
