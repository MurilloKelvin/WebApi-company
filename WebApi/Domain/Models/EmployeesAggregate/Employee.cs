using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Domain.Models.EmployeesAggregate
{
    // Entidade que representa um empregado no banco de dados
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        [Column("ImagePath")]
        public string? Photo { get; set; } // caminho da imagem salva no disco

        public Employee()
        {
        }
    }
}
