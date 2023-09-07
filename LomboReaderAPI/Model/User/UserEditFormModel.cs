namespace LimboReaderAPI.Model.User
{
    public class UserEditFormModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string UserRole { get; set; } = null!;
        public bool Active { get; set; }
    }
}
