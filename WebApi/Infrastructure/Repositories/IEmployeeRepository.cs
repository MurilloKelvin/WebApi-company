using WebApi.Domain.Models.EmployeesAggregate;

namespace WebApi.Infrastructure.Repositories
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync(Employee employee);
        Task<Employee?> GetByIdAsync(int id);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<List<Employee>> GetUserId(int userId);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(Employee employee);
        Task<bool> DesactivateIdAsync(int id);
        Task<bool> ActivateIdAsync(int id);

    }
}
