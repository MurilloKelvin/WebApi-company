using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Application.ViewModel;
using WebApi.Domain.DTO;
using static WebApi.Domain.DTO.EmployeeDTO;
using AutoMapper;
using WebApi.Domain.Models.EmployeesAggregate;
using WebApi.Infrastructure.Repositories;
namespace WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/employee")] // Define a rota da API com versionamento
    [ApiVersion("1.0")] 
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] EmployeeViewModel employeeDTO)
        {
            var employee = _mapper.Map<Employee>(employeeDTO); // Usa o AutoMapper para mapear o EmployeeViewModel para a entidade Employee
            var storagePath = Path.Combine("Storage");

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            var filePath = Path.Combine(storagePath, employeeDTO.Image!.FileName); // Gera o caminho completo para salvar a imagem

            using Stream fileStream = new FileStream(filePath, FileMode.Create); // Cria um stream para salvar a imagem
            await employeeDTO.Image.CopyToAsync(fileStream); // Copia o conteúdo do arquivo de imagem para o stream

            employee.Photo = filePath; // Armazena o caminho da imagem no banco de dados
            await _employeeRepository.AddEmployeeAsync(employee); // Salva o funcionário no banco de dados


            _logger.LogInformation("Employee {Name} added successfully.", employee.Name);
            return Ok(200);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound("Nao encontrado");

            var employeeDTO = _mapper.Map<EmployeeResponseDTO>(employee); // Usa o AutoMapper para mapear a entidade Employee para EmployeeResponseDTO

            return Ok(employeeDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            var employeesDTO = _mapper.Map<List<EmployeeResponseDTO>>(employees); // Usa o AutoMapper para mapear a lista de entidades Employee para uma lista de EmployeeResponseDTO

           // throw new Exception("Teste de erro");

            return Ok(employeesDTO); // pega cada employee da lista e converte para EmployeeResponseDTO usando o método ToViewModel (select é um método de extensão do LINQ que projeta cada elemento de uma sequência em um novo formato)
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

            var dataBytes = System.IO.File.ReadAllBytes(employee.Photo!); // Lê os bytes do arquivo de imagem 

            _logger.LogInformation("Image for Employee with ID {Id} retrieved successfully.", id);
            return File(dataBytes, "image/png"); // Retorna a imagem como um arquivo para download
        }

        [Authorize]
        [HttpPatch("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var deactivated = await _employeeRepository.DesactivateIdAsync(id);

            if (!deactivated)
                return NotFound("Nao encontrado");

            _logger.LogInformation("Employee with ID {Id} deactivated successfully.", id);
            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id:int}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var activated = await _employeeRepository.ActivateIdAsync(id);

            if (!activated)
                return NotFound("Nao encontrado");

            _logger.LogInformation("Employee with ID {Id} activated successfully.", id);
            return NoContent();
        }

    }
}
