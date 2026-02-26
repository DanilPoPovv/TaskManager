using WebApplication1.Requests;
using WebApplication1.Views;

namespace WebApplication1.Services
{
    public interface ITaskService
    {
        public Task<TaskAddView> AddTask(string userId, AddTaskRequest request);
    }
}
