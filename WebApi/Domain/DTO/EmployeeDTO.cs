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
            public int? UserId { get; set; }
            public bool isActive { get; set; }
        }

        public class EmployeeSummaryDTO // DTO para exibir informações resumidas do empregado, como em uma lista
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Age { get; set; }
            public bool isActive { get; set; }
        }

    }
}
