using Microsoft.AspNetCore.Http.HttpResults;
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

        public void AddEmployee(Employee employee)
        {
            _context.EMPRESA.Add(employee);
            _context.SaveChanges();
        }

        public List<Employee> GetAllEmployees()
        {
            return _context.EMPRESA.ToList();
        }
    }
}
