using CleanArchitecture.Domain.DTOs;

namespace CleanArchitecture.Domain.IRepositories
{
    public interface IAccountRepository
    {
        Task<LoginDTO> Login(LoginModel model);
        Task<bool> Register(RegisterModel model);
    }
}
