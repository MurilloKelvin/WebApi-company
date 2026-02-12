namespace WebApi.Domain.Models.EmployeesAggregate
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync(Employee employee);
        Task<Employee?> GetByIdAsync(int id);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<bool> DeleteAsync(int id);
    }
}
