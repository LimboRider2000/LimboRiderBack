namespace LimboReaderAPI.Data.Entety
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? UserRole { get; set; }
        public string? Avatar { get; set; }
        public DateTime? RegisterDt { get; set; }
        public DateTime? LastLoginDt { get; set; } 
        public bool Active { get; set; } = false;
        public string? ActivateCode { get; set; } 

    }
}
