using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApi.Domain.Models.EmployeesAggregate;

namespace WebApi.Application.Services
{
    public class TokenService
    {
        public static object GenerateToken(Employee employee) //rece um employee e retorna um json
        {
            var key = Encoding.ASCII.GetBytes(Key.Secret); // converte a chave secreta para bytes
            var tokenConfig = new SecurityTokenDescriptor //cria um objeto com as configurações do token "quem e o usuario"
            {
                Subject = new ClaimsIdentity(new Claim[] // informações que serão incluídas no token
                {
                    new Claim("employeeId", employee.Id.ToString()), // id do funcionário
                }),
                Expires = DateTime.UtcNow.AddHours(3), // token válido por 3 hora
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // algoritmo de assinatura do token
            };

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig); // criação do do manipulador de token e criação do token com as configurações definidas
            var tokenString = tokenHandler.WriteToken(token); // conversão do token para string

            return new
            {
                token = tokenString }; // retorno do token em formato JSON
            }
    }
}
