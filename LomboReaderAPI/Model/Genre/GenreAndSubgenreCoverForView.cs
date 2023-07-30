namespace LimboReaderAPI.Model.Genre
{
    public class GenreAndSubgenreCoverForView
    {
        public GenreAndSubgenreCoverForView() {
           SubGenreList = new List<Data.Entety.SubGenre>();
        }   
        public Data.Entety.Genre Genre { get; set; } = null!;
        public List<Data.Entety.SubGenre> SubGenreList { get; set; }


    }
}
