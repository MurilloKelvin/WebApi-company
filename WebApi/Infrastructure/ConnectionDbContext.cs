using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
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
             .HasOne(e => e.User)
             .WithOne(u => u.Employee)
             .HasForeignKey<Employee>(f => f.UserId);
        }

    }
}
