using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebApi.Domain.Models.EmployeesAggregate;

namespace WebApi.Infrastructure
{
    // Contexto do banco de dados, faz a ponte entre a aplicação e o MySQL
    public class ConnectionDbContext : DbContext
    {
        public ConnectionDbContext(DbContextOptions<ConnectionDbContext> options) : base(options) { }

        public DbSet<Employee> EMPRESA { get; set; } // tabela de empregados no banco

    }
}
