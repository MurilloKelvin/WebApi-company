using WebApi.Application.ViewModel;
using WebApi.Domain.Models;

namespace WebApi.Domain.DTO
{
    public class EmployeeDTO
    {
        public class EmployeeResponseDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Age { get; set; }
            public string? Photo { get; set; }
        }

    }
}
