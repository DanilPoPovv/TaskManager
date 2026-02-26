using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Requests;
using WebApplication1.Services;
using WebApplication1.Views;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase 
    {
        private readonly ITaskService taskService;

        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpPost("Add")]
        [Authorize(Roles = "Admin,User")]
        public async Task<TaskAddView> AddTask(AddTaskRequest request)
        {
            var task = await taskService.AddTask(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,request);
            return task;
        }
        
    }
}
