namespace CleanArchitecture.Domain.DTOs
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool RememberMe { get; set; }
    }
}
