using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using WebApi.Application.Services;
using WebApi.Domain.Models;
using WebApi.Infrastructure.Repositories;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository; // Injeta o repositório de usuários para acessar os dados dos usuários
        }

        public class AuthenticationRequest
        {
            public string Username { get; set; } = string.Empty; // Propriedade para armazenar o nome de usuário enviado na requisição
            public string Password { get; set; } = string.Empty; // Propriedade para armazenar a senha enviada na requisição
        }

        // Autentica o usuário e retorna um token JWT se as credenciais estiverem corretas
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            var user = await _userRepository.GetUsernameAsync(request.Username); // Busca o usuário no banco de dados pelo nome de usuário
            if (user == null || !user.IsActive) 
                return Unauthorized("Usuário inativo ou não encontrado."); // Retorna 401 Unauthorized se o usuário não for encontrado ou estiver inativo
            
            var ok = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash); // Verifica se a senha fornecida corresponde ao hash armazenado
            if (!ok)
                return Unauthorized("Senha incorreta."); // Retorna 401 Unauthorized se a senha estiver incorreta

            var token = TokenService.GenerateToken(user); // Gera um token JWT para o usuário autenticado

            return Ok(token); // Retorna o token em formato JSON
        }
    }
}
