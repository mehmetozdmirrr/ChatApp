

namespace ChatApp.Models

{
    public sealed class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public string? Avatar { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;
        public string refreshToken { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
