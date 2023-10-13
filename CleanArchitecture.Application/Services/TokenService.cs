using CleanArchitecture.Domain.DTOs;
using CleanArchitecture.Domain.IRepositories;
using CleanArchitecture.Domain.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CleanArchitecture.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IUserRepository _userRepository;

        public TokenService(IUserRepository userRepository)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BDF4D2DB60514A7592D3B42008EC84A2BDF4D2DB60514A7592D3B42008EC84A2"));
            _userRepository = userRepository;
        }

        private SymmetricSecurityKey GenerateRandom512BitKey()
        {
            var keyBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }

            return new SymmetricSecurityKey(keyBytes);
        }

        public string CreateToken(LoginModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username)
            };

            var roles = _userRepository.GetUserRoleByUserNameAsync(user.Username).Result;

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}