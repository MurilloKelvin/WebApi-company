using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApi.Models;

namespace WebApi.Models
{
    // Entidade que representa um empregado no banco de dados
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? ImagePath { get; set; } // caminho da imagem salva no disco


        [NotMapped] // não salva no banco de dados
        [JsonIgnore] // não aparece no retorno da API
        public IFormFile? Image { get; set; }

        public Employee()
        {
        }
    }
}
