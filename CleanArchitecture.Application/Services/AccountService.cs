using CleanArchitecture.Domain.DTOs;
using CleanArchitecture.Domain.IRepositories;
using CleanArchitecture.Domain.IServices;

namespace CleanArchitecture.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<LoginDTO> Login(LoginModel model)
        {
            return await _accountRepository.Login(model);
        }

        public async Task<bool> Register(RegisterModel model)
        {
            return await _accountRepository.Register(model);
        }
    }
}
