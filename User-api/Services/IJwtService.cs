using User_api.Models;

namespace User_api.Services
{
    public interface IJwtService
    {
        string GenerateToken(UserModel user);
    }
}
