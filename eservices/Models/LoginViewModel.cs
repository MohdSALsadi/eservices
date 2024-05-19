namespace eservices.Models
{
    public class LoginViewModel
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required int Role { get; set; }
        public required bool RememberMe { get; set; }
    }
}
