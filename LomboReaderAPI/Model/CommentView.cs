using LimboReaderAPI.Data.Entety;

namespace LimboReaderAPI.Model
{
    public class CommentView : Comments
    {
        public Data.Entety.User userObj { get; set; } = null!; 
    }
}
