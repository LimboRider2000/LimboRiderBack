namespace LimboReaderAPI.Data.Entety
{
    public class Comments
    {
        public Guid Id { get; set; }
        public Guid BookArticle_Id { get; set; }
        public Guid User { get; set; }
        public DateTime DateTime { get; set; }
        public string Comment { get; set; } = null!;

    }
}
