using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApi.Models;

namespace WebApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? ImagePath { get; set; }


        [NotMapped] // propiedade pra nao listar isso no DB e nao criar um JSON no retorno da API
        [JsonIgnore]
        public IFormFile? Image { get; set; }

        public Employee()
        {
        }
    }
}
