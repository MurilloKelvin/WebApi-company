using WebApi.Domain.Models.EmployeesAggregate;

namespace WebApi.Infrastructure.Repositories
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync(Employee employee);
        Task<Employee?> GetByIdAsync(int id);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<bool> DeleteAsync(int id);
    }
}
