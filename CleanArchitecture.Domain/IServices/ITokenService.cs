using CleanArchitecture.Domain.DTOs;

namespace CleanArchitecture.Domain.IServices
{
    public  interface ITokenService
    {
        string CreateToken(LoginModel user);
    }
}
