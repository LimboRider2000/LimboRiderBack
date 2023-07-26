namespace LomboReaderAPI.Data.Entety
{
    public class SubGenre
    {
        public Guid Id { get; set; }
        public Guid Genre_id { get; set; }
        public string SubGenreName { get; set; } = null!;

    }
}
