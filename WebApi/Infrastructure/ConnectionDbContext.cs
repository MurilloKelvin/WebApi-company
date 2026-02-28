using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Models.EmployeesAggregate;
using WebApi.Domain.Models;

namespace WebApi.Infrastructure
{
    // Contexto do banco de dados, faz a ponte entre a aplicação e o MySQL
    public class ConnectionDbContext : DbContext
    {
        public ConnectionDbContext(DbContextOptions<ConnectionDbContext> options) : base(options) { }

        public DbSet<Employee> EMPRESA { get; set; } // tabela de empregados no banco

        public DbSet<User> USERS { get; set; } // tabela de usuários no banco

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            modelBuilder.Entity<Employee>()
             .HasOne(e => e.User) // Configura a relação entre Employee e User, indicando que um empregado tem um usuário associado
             .WithMany(u => u.Employees) // Configura a relação entre Employee e User, indicando que um usuário pode ter muitos empregados
             .HasForeignKey(e => e.UserId) // Configura a chave estrangeira para a relação entre Employee e User
             .IsRequired(); // Configura a relação entre Employee e User, garantindo que cada empregado esteja associado a um usuário existente
        }

    }
}
