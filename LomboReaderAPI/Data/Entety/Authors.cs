namespace LimboReaderAPI.Data.Entety
{
    public class Authors
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? SecondName { get; set; }
    }
}
