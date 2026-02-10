using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebApi.Models;

namespace WebApi.Infrastructure
{
    public class ConnectionDbContext : DbContext
    {
        public ConnectionDbContext(DbContextOptions<ConnectionDbContext> options) : base(options) { }

        public DbSet<Employee> EMPRESA { get; set; } // crie uma propriedade DbSet para a entidade Employee, nomeada como EMPRESA

    }
}
