using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

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
                var token = TokenService.GenerateToken(new Models.Employee());

                return Ok(token);
            }
            else
            {
                return Unauthorized(new { message = "Credenciais inválidas" });
            }
        }
    }
}
