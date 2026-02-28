using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.ViewModel;
using WebApi.Domain.DTO;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserViewModel userDTO)
        {
            var user = _mapper.Map<User>(userDTO); // Usa o AutoMapper para mapear o UserViewModel para a entidade User
            await _userRepository.AddUserAsync(user); // Salva o usuário no banco de dados

            _logger.LogInformation("User {Username} added successfully.", user.Username);

            return Created();
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return NotFound("Nao encontrado");

            var userDTO = _mapper.Map<UserResponseDTO>(user);
            return Ok(userDTO);

        }

        [Authorize]
        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
                return NotFound("Nao encontrado");

            var userDTO = _mapper.Map<UserResponseDTO>(user);
            return Ok(userDTO);
        }

        [Authorize]
        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetUsernameAsync(username);

            if (user == null)
                return NotFound("Nao encontrado");

            var userDTO = _mapper.Map<UserResponseDTO>(user);
            return Ok(userDTO);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var result = await _userRepository.DeleteUserAsync(id);

            if (!result)
                return NotFound("Nao encontrado");

            _logger.LogInformation("User with ID {UserId} deleted successfully.", id);
            return Ok("User deletado com sucesso");
        }

        [Authorize]
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
