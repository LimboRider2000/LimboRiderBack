namespace LimboReaderAPI.Data.Entety
{
    public class BookArticle
    {
        public Guid        Id            { get; set; }
        public User        User_id { get; set; } = null!;
        public string?     TitleImgPath  { get; set; }
        public string      Title         { get; set; } = null!;
        public string      Description   { get; set; } = null!;
        public string      FilePath      { get; set; } = null!;
        public DateTime    CreatedDate   { get; set; }
        public Authors?    Author_id     { get; set; }
        public Genre       Genre_id { get; set; } = null!;
        public SubGenre    SubGenre_id { get; set; } = null!; 
        public double      Rating        { get; set; }
    }
}
