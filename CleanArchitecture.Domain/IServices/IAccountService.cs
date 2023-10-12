using CleanArchitecture.Domain.DTOs;

namespace CleanArchitecture.Domain.IServices
{
    public interface IAccountService
    {
        Task<bool> Register(RegisterModel model);
        Task<LoginDTO> Login(LoginModel model);
    }
}
