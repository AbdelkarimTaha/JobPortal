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
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _accountService.Register(model);
            if (result)
            {
                return Ok(new { Message = "Registration successful" });
            }
            return BadRequest(new { Message = "Registration failed" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var loginDto = await _accountService.Login(model);
            if (loginDto != null)
            {
                return Ok(loginDto);
            }
            return Unauthorized(new { Message = "Login failed" });
        }
    }
}