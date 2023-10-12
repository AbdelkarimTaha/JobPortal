using JWT.API.Models;

namespace JWT.API.Repository
{
    public interface ITokenRepository
    {
        Tokens Authenticate(Users users);
    }
}
