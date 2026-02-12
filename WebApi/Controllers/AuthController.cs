using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Services;
using WebApi.Domain.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        // Autentica o usuário e retorna um token JWT se as credenciais estiverem corretas
        [HttpPost]
        public IActionResult Auth(string username, string password)
            {
            if (username == "admin" && password == "password")
            {
                var token = TokenService.GenerateToken(new Employee());

                return Ok(token);
            }
            else
            {
                return Unauthorized(new { message = "Credenciais inválidas" });
            }
        }
    }
}
