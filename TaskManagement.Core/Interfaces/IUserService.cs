using TaskManagement.Core.Entities;

namespace TaskManagement.Core.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(string name, string email, string role);
    Task<User> UpdateUserAsync(int id, string name, string email);
    Task<bool> DeleteUserAsync(int id);
}