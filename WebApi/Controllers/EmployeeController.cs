using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApi.Infrastructure;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {

        private readonly ConnectionDbContext _context;

        public EmployeeController(ConnectionDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Employee employee)
        {

            var storagePath = Path.Combine("Storage"); // Define o caminho para a pasta de armazenamento

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            var filePath = Path.Combine(storagePath, employee.Image.FileName); // Define o caminho completo para o arquivo a ser salvo

            using Stream FileStream = new FileStream(filePath, FileMode.Create); // Cria um FileStream para salvar o arquivo

            await employee.Image.CopyToAsync(FileStream); // Copia o conteúdo do arquivo enviado para o FileStream
            employee.ImagePath = filePath; // Atribui o caminho do arquivo salvo à propriedade ImagePath do funcionário


            _context.EMPRESA.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(200);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.EMPRESA.FindAsync(id); // Busca o funcionário pelo ID usando FindAsync

            if (employee == null)
                return NotFound("Nao encontrado");

            return Ok(employee);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _context.EMPRESA.ToListAsync(); // Busca todos os funcionários usando ToListAsync
            return Ok(employees);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await _context.EMPRESA.FindAsync(id);

            if (employee == null)
                return NotFound("Nao encontrado");

            _context.EMPRESA.Remove(employee); // Remove o funcionário do contexto
            await _context.SaveChangesAsync();


            return NoContent();
        }


        [HttpGet]
        [Route("{id}/download")]
        public async Task<IActionResult> GetImage(int id)
        {
            var employee = await _context.EMPRESA.FindAsync(id);

            if (employee == null)
                return NotFound("Nao encontrado");

            var dataBytes = System.IO.File.ReadAllBytes(employee.ImagePath); // Lê o conteúdo do arquivo de imagem como um array de bytes

            return File(dataBytes, "image/png"); // Retorna o arquivo de imagem 
        }

    } 
}
