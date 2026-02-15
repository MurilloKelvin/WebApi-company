using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Domain.Models;

namespace WebApi.Domain.Models.EmployeesAggregate
{
    // Entidade que representa um empregado no banco de dados
    [Table("Employees")]
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        [Column("ImagePath")]
        public string? Photo { get; set; } // caminho da imagem salva no disco

        public int? UserId { get; set; } // chave estrangeira para a tabela de usuários

        public User User { get; set; } // referência para o usuário associado ao empregado


        public Employee()
        {
        }
    }
}
