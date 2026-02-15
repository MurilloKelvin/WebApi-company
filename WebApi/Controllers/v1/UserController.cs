using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.ViewModel;
using WebApi.Domain.Models;
using WebApi.Infrastructure.Repositories;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/user")] // Define a rota da API com versionamento
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository, IMapper mapper)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserViewModel userDTO)
        {
            var user = _mapper.Map<User>(userDTO); // Usa o AutoMapper para mapear o UserViewModel para a entidade User
            await _userRepository.AddUserAsync(user); // Salva o usuário no banco de dados

            _logger.LogInformation("User {Username} added successfully.", user.Username);

            return Ok(200);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            var userDTO = _mapper.Map<UserViewModel>(user); // Usa o AutoMapper para mapear a entidade User para UserViewModel

            if (user == null)
                return NotFound("Nao encontrado");

            return Ok(user);

        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            var userDTO = _mapper.Map<UserViewModel>(user); // Usa o AutoMapper para mapear a entidade User para UserViewModel

            if (user == null)
                return NotFound("Nao encontrado");

            return Ok(userDTO);
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetUsernameAsync(username);

            var userDTO = _mapper.Map<UserViewModel>(user); // Usa o AutoMapper para mapear a entidade User para UserViewModel

            if (user == null)
                return NotFound("Nao encontrado");

            return Ok(userDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var result = await _userRepository.DeleteUserAsync(id);

            if (!result)
                return NotFound("Nao encontrado");

            _logger.LogInformation("User with ID {UserId} deleted successfully.", id);
            return Ok("User deletado com sucesso");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserViewModel user)
        {
            var userExists = await _userRepository.GetUserByIdAsync(user.Id); // Verifica se o usuário existe no banco de dados
            var userDTO = _mapper.Map<UserViewModel>(user); // Usa o AutoMapper para mapear o UserViewModel para a entidade User

            var result = await _userRepository.UpdateUserAsync(userDTO); // Atualiza o usuário no banco de dados

            if (!result)
                return NotFound("Nao encontrado");
            _logger.LogInformation("User {Username} updated successfully.", user.Username);
            return Ok(200);
        }



    }
}