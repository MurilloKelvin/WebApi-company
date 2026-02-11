namespace WebApi.Models
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync(Employee employee);
        Task<Employee?> GetByIdAsync(int id);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<bool> DeleteAsync(int id);
    }
}
