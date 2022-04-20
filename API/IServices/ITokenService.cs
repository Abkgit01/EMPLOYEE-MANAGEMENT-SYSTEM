using API.Models;

namespace API.IServices
{
    public interface ITokenService
    {
        public string CreateToken(AppUser user);
    }
}
