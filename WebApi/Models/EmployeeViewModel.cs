namespace WebApi.Models
{
    public class EmployeeViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class EmployeeResponseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? ImagePath { get; set; }
    }
}
