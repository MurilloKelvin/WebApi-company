using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Infrastructure;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] EmployeeViewModel employeeVm)
        {
            var employee = ToEntity(employeeVm);
            var storagePath = Path.Combine("Storage");

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            var filePath = Path.Combine(storagePath, employeeVm.Image!.FileName);

            using Stream fileStream = new FileStream(filePath, FileMode.Create);
            await employeeVm.Image.CopyToAsync(fileStream);

            employee.ImagePath = filePath;
            await _employeeRepository.AddEmployeeAsync(employee);


            _logger.LogInformation("Employee {Name} added successfully.", employee.Name);
            return Ok(200);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound("Nao encontrado");
            }

            return Ok(ToViewModel(employee));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            _logger.LogInformation("Teste");

           // throw new Exception("Teste de erro");

            return Ok(employees.Select(ToViewModel));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _employeeRepository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound("Nao encontrado");
            }

            _logger.LogInformation("Employee with ID {Id} deleted successfully.", id);
            return NoContent();
        }

        [Authorize]
        [HttpGet]
        [Route("{id}/download")]
        public async Task<IActionResult> GetImage(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound("Nao encontrado");
            }

            var dataBytes = System.IO.File.ReadAllBytes(employee.ImagePath!);

            _logger.LogInformation("Image for Employee with ID {Id} retrieved successfully.", id);
            return File(dataBytes, "image/png");
        }

        private static EmployeeResponseViewModel ToViewModel(Employee employee)
        {
            return new EmployeeResponseViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                ImagePath = employee.ImagePath
            };
        }

        private static Employee ToEntity(EmployeeViewModel employeeVm)
        {
            return new Employee
            {
                Name = employeeVm.Name,
                Age = employeeVm.Age,
                Image = employeeVm.Image
            };
        }
    }
}
