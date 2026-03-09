using Microsoft.EntityFrameworkCore;
using WebApplication1.EntityFramework;
using WebApplication1.Helpers;
using WebApplication1.Requests;
using WebApplication1.Validators;
using WebApplication1.Validators.Models;
using WebApplication1.Views;

namespace WebApplication1.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _dbContext;
        private readonly IValidator<AddTaskValidationModel> _addTaskValidator;
        private readonly IUserProvider _userProvider;
        public TaskService(AppDbContext dbContext, IUserProvider userProvider, IValidator<AddTaskValidationModel> addValidator)
        {
            this._dbContext = dbContext;
            _addTaskValidator = addValidator;
            _userProvider = userProvider;
        }

        public async Task<TaskAddView> AddTask(string userId, AddTaskRequest request)
        {
            var user = await _userProvider.GetByIdOrThrow(userId);
            await _addTaskValidator.Validate(new AddTaskValidationModel() { Request = request, UserId = user.Id });
            AppTask task = await AddTaskToDb(user, request);
            return new TaskAddView(task);
        }
        private async Task<AppTask> AddTaskToDb(User user, AddTaskRequest request)
        {
            var task = new AppTask() { Name = request.Name, Description = request.Description, CreatedAt = DateTime.UtcNow, UserId = user.Id};
            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();
            return task;
        }
    }
}
