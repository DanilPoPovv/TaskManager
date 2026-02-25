namespace WebApplication1.Requests
{
    public class RegisterRequest 
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SecondName { get; set; }
        public bool IsUserAdmin { get; set; }
    }
}
