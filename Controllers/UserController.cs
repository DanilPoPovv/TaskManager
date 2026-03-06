using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using WebApplication1.Requests;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
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
        [HttpGet]
        [Route("GetUsersByMinLength")]
        public IActionResult GetUsersByMinLength(int length)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            var users = new List<object>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using(SqlCommand cmd = new SqlCommand("GetUsersWithNameLongerThan", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MinLength", length);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            users.Add(new
                            {
                                Id = reader["Id"],
                                Name = reader["UserName"]
                            });
                        }
                    }
                }
            }
            return Ok(users);
        }
    }
}
