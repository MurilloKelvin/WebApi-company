using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(ConnectionDbContext context)
        {
            _employeeRepository = new EmployeeRepository(context);
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

            return Ok(200);
        }

        [Authorize]
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
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
