namespace WebApplication1.Views
{
    public class TaskAddView
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public TaskAddView(AppTask task)
        {
            Name = task.Name;
            Description = task.Description;
            CreationDate = task.CreatedAt;
        }
    }
}
