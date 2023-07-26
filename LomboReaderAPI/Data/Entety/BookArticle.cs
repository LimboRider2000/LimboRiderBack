namespace LomboReaderAPI.Data.Entety
{
    public class BookArticle
    {
        public Guid        Id            { get; set; }
        public Guid        User_id       { get; set; }
        public string?     TitleImgFile  { get; set; }
        public string      Title         { get; set; } = null!;
        public string      Description   { get; set; } = null!;
        public string      FilePath      { get; set; } = null!;
        public DateTime    CreatedDate   { get; set; }
        public Guid?       Author_id     { get; set; }
        public List<Genre>?GenresList { get; set; }
        public int         Rating        { get; set; }
        public List<Tag>?  TagList    { get; set; }
    }
}
