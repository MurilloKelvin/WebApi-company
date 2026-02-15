namespace WebApi.Domain.DTO
{
    public class AuthDTO
    {
        public class LoginRequestDTO
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;

        }

        public class RegisterRequestDTO
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }

        public class AuthResponseDTO
        {
            public string Token { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public DateTime Expiration { get; set; }
        }
    }
}
