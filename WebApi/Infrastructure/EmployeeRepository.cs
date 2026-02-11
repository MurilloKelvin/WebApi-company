using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Infrastructure
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
            _context.EMPRESA.Add(employee);
            await _context.SaveChangesAsync();
        }

        public Task<Employee?> GetByIdAsync(int id)
        {
            return _context.EMPRESA.FindAsync(id).AsTask();
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
    }
}
