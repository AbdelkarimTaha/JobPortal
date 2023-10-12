using CleanArchitecture.Domain.DTOs;
using CleanArchitecture.Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService, IConfiguration config)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public Task<bool> Register(RegisterModel model)
        {
            return _accountService.Register(model);
        }

        [HttpPost("login")]
        public Task<LoginDTO> Login(LoginModel model)
        {
            return _accountService.Login(model);
        }
    }
}