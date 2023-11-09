using LimboReaderAPI.Data;
using LimboReaderAPI.Data.Entety;

using Microsoft.EntityFrameworkCore;

namespace LimboReaderAPI.Repositories
{
    public class BookRepositories : IBookRepositories
    {
        private readonly DataContext dataContext;
        public BookRepositories(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BookArticle>> Get(int _offset = 0, int _perPage = 0)    
        {
              return await dataContext.BookArticles
                              .Include(a => a.Author_id)
                              .Include(u => u.User_id)
                              .Include(g => g.Genre_id)
                              .Include(s => s.SubGenre_id)
                              .OrderByDescending(t => t.CreatedDate)
                              .Skip(_offset)
                              .Take(_perPage)
                              .ToListAsync();
        }

        public async Task<BookArticle?> Get(string id)
        {
           return await dataContext.BookArticles.Where(item => item.Id == Guid.Parse(id))
                     .Include(a => a.Author_id)
                     .Include(u => u.User_id)
                     .Include(g => g.Genre_id)
                     .Include(s => s.SubGenre_id).FirstOrDefaultAsync();
            
        }
        public async Task<List<BookArticle>> GetSG( string findBySybGenre, int _offset = 0, int _perPage = 0) {
            return await dataContext.BookArticles
             .Include(a => a.Author_id)
             .Include(u => u.User_id)
             .Include(g => g.Genre_id)
             .Include(s => s.SubGenre_id)
             .Where(item => item.SubGenre_id.Id == Guid.Parse(findBySybGenre))
             .Skip(_offset)
             .Take(_perPage)
             .ToListAsync();
        }

        public async Task<int> GetCount()
        {
                return await dataContext.BookArticles.CountAsync();
        }

        public async Task Post(BookArticle data)
        {
             await dataContext.BookArticles.AddAsync(data);
        }

        public Task<BookArticle> Put(BookArticle data)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetCountBuSubGenre(string subGenreId)
        {
           return await dataContext.BookArticles
                 .Where(item => item.SubGenre_id.Id == Guid
                                                    .Parse(subGenreId)).CountAsync();
        }

        public async Task<List<BookArticle>> GetByUserId(string id)
        {

           return await dataContext.BookArticles
                .Include(a => a.Author_id)
                .Include(u => u.User_id)
                .Include(g => g.Genre_id)
                .Include(s => s.SubGenre_id)
                .Where(item => item.User_id.Id == Guid.Parse(id))
                .ToListAsync();
        }
    }
}
