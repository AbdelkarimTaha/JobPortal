namespace CleanArchitecture.Domain.DTOs
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsEmployer { get; set; }
    }
}
