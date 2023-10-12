using JWT.API.Models;
using JWT.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JWTController : ControllerBase
    {
        public ITokenRepository _tokenRepository;

        public JWTController(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        [HttpGet]
        public List<string> Get() 
        {
            return new List<string>
            {
                "John Doe",
                "Jane Doe",
                "Junior Doe"
            };
        }


        [HttpPost]
        [Route("authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate(Users usersdata)
        {
            var token = _tokenRepository.Authenticate(usersdata);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}