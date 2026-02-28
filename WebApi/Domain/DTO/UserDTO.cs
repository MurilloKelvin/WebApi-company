namespace WebApi.Domain.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // User, Admin, Manager

        public string CompanyName { get; set; } = string.Empty;
    }

    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // User, Admin, Manager
        public string CompanyName { get; set; } = string.Empty;

        public List<EmployeeDTO.EmployeeSummaryDTO> Employees { get; set; } = new();
    }
}
