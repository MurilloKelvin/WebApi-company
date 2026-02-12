using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Domain.Models.CompanyAggregate
{
    [Table("Company")]
    public class Company
    {
        [Key]
        int Id { get; set; }
        string Name { get; set; } = string.Empty;
    }
}
