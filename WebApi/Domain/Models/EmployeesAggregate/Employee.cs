using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Domain.Models.EmployeesAggregate
{
    // Entidade que representa um empregado no banco de dados
    [Table("Employees")]
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public bool isActive { get; set; } = true; // campo para indicar se o empregado está ativo ou inativo

        [Column("ImagePath")]
        public string? Photo { get; set; } // caminho da imagem salva no disco

        public int UserId { get; set; } // chave estrangeira para a tabela de usuários

        public User User { get; set; } = null!; // referência para o usuário associado ao empregado inicia null para evitar problemas de referência circular




        public Employee()
        {
        }
    }
}
