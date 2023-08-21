namespace LimboReaderAPI.Model.User
{
    public class UserFormModel
    {
        public string login { get; set; } = null!;
        public string password { get; set; } = null!;
        public string email { get; set; } = null!;
        public string name { get; set; } = null!;
        public string? avatar { get; set; }
    }
}
