using LimboReaderAPI.Data.Entety;

namespace LimboReaderAPI.Model.Book
{
    public class BookTransfer
    {
        public Guid Id { get; set; }
        public string User_name { get; set; } = null!;
        public string? TitleImgPath { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public Authors Author { get; set; } = null!;
        public string Genre_name { get; set; } = null!;
        public string SubGenre_name { get; set; } = null!;
        public double Rating { get; set; }
    }
}
