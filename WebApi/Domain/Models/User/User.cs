using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Domain.Models.EmployeesAggregate;

namespace WebApi.Domain.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // User, Admin, Manager
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; } = true;

        public List<Employee>? Employees { get; set; } // referência para o empregado associado ao usuário
    }
}