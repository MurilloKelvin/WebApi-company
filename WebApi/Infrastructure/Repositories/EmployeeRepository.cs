using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Models.EmployeesAggregate;

namespace WebApi.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ConnectionDbContext _context;

        public EmployeeRepository(ConnectionDbContext context)
        {
            _context = context;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            var existingUser = await _context.USERS.FindAsync(employee.UserId);
            

            if (existingUser == null)
            {
                throw new Exception($"User with ID {employee.UserId} does not exist.");
            }

            _context.EMPRESA.Add(employee);
            await _context.SaveChangesAsync();
        }

        public Task<Employee?> GetByIdAsync(int id)
        {
            return _context.EMPRESA.FindAsync(id).AsTask();
        }

        public Task<List<Employee>> GetUserId(int userId)
        {
            return _context.EMPRESA.Where(e => e.UserId == userId).ToListAsync(); // e.userId é a propriedade que relaciona o funcionário ao usuário
        }

        public Task<List<Employee>> GetAllEmployeesAsync()
        {
            return _context.EMPRESA.ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.EMPRESA.FindAsync(id);

            if (employee == null)
            {
                return false;
            }

            _context.EMPRESA.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            var existingEmployee = await _context.EMPRESA.FindAsync(employee.Id);
            if (existingEmployee == null)
            {
                return false;
            }
            existingEmployee.Name = employee.Name;
            existingEmployee.Photo = employee.Photo;
            existingEmployee.UserId = employee.UserId;
            existingEmployee.isActive = employee.isActive;
            _context.EMPRESA.Update(existingEmployee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DesactivateIdAsync(int id)
        {
            var existingEmployee = await _context.EMPRESA.FindAsync(id);
            if(existingEmployee ==null)
                return false;

            existingEmployee.isActive = false;
            _context.EMPRESA.Update(existingEmployee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateIdAsync(int id)
        {
            var existingEmployee = await _context.EMPRESA.FindAsync(id);
            if(existingEmployee ==null)
                return false;

            existingEmployee.isActive = true;
            _context.EMPRESA.Update(existingEmployee);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
