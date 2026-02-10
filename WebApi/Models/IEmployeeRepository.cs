namespace WebApi.Models
{
    public interface IEmployeeRepository
    {
        void AddEmployee(Employee employee);

        List<Employee>  GetAllEmployees();

        void Delete(int Id);
    }
}
