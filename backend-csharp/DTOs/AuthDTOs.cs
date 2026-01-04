namespace PrisonManagement.DTOs
{
    public class LoginRequestDTO
    {
        public string Ten { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
    }

    public class LoginResponseDTO
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public AuthUserDTO? User { get; set; }
        public string? Token { get; set; }
    }

    public class AuthUserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? CanBoId { get; set; }
    }
}
