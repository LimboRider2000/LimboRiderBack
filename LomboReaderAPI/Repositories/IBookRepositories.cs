using LimboReaderAPI.Data.Entety;

using Microsoft.AspNetCore.Components.Web;

namespace LimboReaderAPI.Repositories
{
    public interface IBookRepositories
    {
        Task<List<BookArticle>>
            Get(int _offset = 0, int _perPage = 0);
        Task<BookArticle?> Get(string id);
        Task<List<BookArticle>> GetByUserId(string id);

        Task<List<BookArticle>> 
            GetSG(string findBySybGenre, 
            int _offset = 0, int _perPage = 0);

        Task Post(BookArticle data);
        Task<bool> Delete(string id);
        Task<BookArticle> Put(BookArticle data);
        Task<int> GetCount();
        Task<int> GetCountBuSubGenre(string id);
    }
}
