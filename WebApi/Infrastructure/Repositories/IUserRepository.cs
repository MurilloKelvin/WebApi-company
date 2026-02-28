
using WebApi.Application.ViewModel;
using WebApi.Domain.Models;

namespace WebApi.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUsernameAsync(string username);
        Task<User> AddUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> UpdateUserAsync(User userDTO);
    }
}
