using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.ViewModel;
using WebApi.Domain.Models;

namespace WebApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ConnectionDbContext _context;

        public UserRepository(ConnectionDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            _context.USERS.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.USERS.FindAsync(userId);

            if (user == null)
                return false;

            _context.USERS.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _context.USERS
                .Include(u => u.Employees)
                .FirstOrDefaultAsync(user => user.Email == email);

            if (user == null)
                return null;

            return user;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _context.USERS
                .Include(u => u.Employees)
                .FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
                return null;

            return user;
        }

        public async Task<User?> GetUsernameAsync(string username)
        {
            var user = await _context.USERS
                .Include(u => u.Employees)
                .FirstOrDefaultAsync(user => user.Username == username);

            if (user == null)
                return null;

            return user;
        }

        public async Task<bool> UpdateUserAsync(User updatedUser)
        {
            var user = await _context.USERS.FindAsync(updatedUser.Id);

            if (user == null)
                return false;

            _context.Entry(user).CurrentValues.SetValues(updatedUser);

            await _context.SaveChangesAsync();
            return true;

        }


    }
}
