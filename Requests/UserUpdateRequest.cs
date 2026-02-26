namespace WebApplication1.Requests
{
    public class UserUpdateRequest
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
