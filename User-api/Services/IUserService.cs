using User_api.Models;

namespace User_api.Services
{
    public interface IUserService
    {
        Task<UserModel?> CreateUserAsync(UserModel user);
        Task<UserModel?> FindByEmailAsync(string email);
        Task<List<UserModel>> GetAllUsersAsync();
        Task<UserModel?> GetUserByIdAsync(int id);
        Task<UserModel?> UpdateUserAsync(int id, UserModel model);
        Task<UserModel?> DeledeUserAsync(int id);
        Task<UserModel> AuthenticateAsync(string email, string password);
    }
}
