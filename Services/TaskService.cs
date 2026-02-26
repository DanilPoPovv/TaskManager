using Microsoft.EntityFrameworkCore;
using WebApplication1.EntityFramework;
using WebApplication1.Requests;
using WebApplication1.Views;

namespace WebApplication1.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext dbContext;

        public TaskService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<TaskAddView> AddTask(string userId, AddTaskRequest request)
        {
            var user = await dbContext.Users.Include(x => x.Tasks).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new Exception("Your claims is invalid");
            if (user.Tasks.Any(x => x.Name == request.Name))
                throw new Exception($"Task with name {request.Name} is already created");
            var task = new AppTask() { Name = request.Name, Description = request.Description, CreatedAt = DateTime.Now.Date, User = user };
            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();
            return new TaskAddView(task);
        }
        
    }
}
