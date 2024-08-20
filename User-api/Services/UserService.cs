using User_api.Models;
using User_api.Data;
using Microsoft.EntityFrameworkCore;

namespace User_api.Services
{
    public class UserService(AplicationDbContext context) : IUserService
    {
        public async Task<UserModel?> CreateUserAsync(UserModel user)
        {
            if (await FindByEmailAsync(user.Email) != null)
                throw new Exception($"The user by email {user.Email} is already exist");
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;

        }

        public async Task<UserModel?> FindByEmailAsync(string email) =>
            await context.Users.FirstOrDefaultAsync(x => x.Email == email);

        public async Task<List<UserModel>> GetAllUsersAsync() =>
            await context.Users.ToListAsync();

        public async Task<UserModel?> GetUserByIdAsync(int id) =>
            await context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<UserModel?> UpdateUserAsync(int id, UserModel model)
        {
            UserModel byId = await GetUserByIdAsync(id) ?? throw new Exception($"User by id {id} is does not found");
                byId.Id = id;
                byId.Name = model.Name;
                byId.Email = model.Email;
                byId.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                await context.SaveChangesAsync();
            return byId;
        }

        public async Task<UserModel?> DeledeUserAsync(int id)
        {
            UserModel byId = await GetUserByIdAsync(id) ?? throw new Exception($"User by id {id} is does not found");
            context.Users.Remove(byId);
            await context.SaveChangesAsync();
            return byId;
        }

        public async Task<UserModel> AuthenticateAsync(string email, string password)
        {
            var user = await FindByEmailAsync(email) ?? throw new Exception($"The user by email {email} does not found");
            var isValidPwd = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!isValidPwd)
            {
                throw new Exception("Wrong connection details");
            }
            return user;
        }

        
    }
}
