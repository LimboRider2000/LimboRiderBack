using LimboReaderAPI.Data;
using LimboReaderAPI.Data.Entety;

using LimboReaderAPI.Model.Book;
using LimboReaderAPI.Services.File;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;

using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LimboReaderAPI.Controllers.BookPost
{
    [Route("api/Book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        IFileWriter _fileWriter;
        DataContext _dataContext;
        private int _perPage = 5;
        public BookController(IFileWriter fileWriter, DataContext dataContext)
        {
            _fileWriter = fileWriter;
            _dataContext = dataContext;
        }

        // GET: api/<BookController>
        [HttpGet]
        public async Task<ActionResult> Get(int page = 1)
        {
            if (page < 1) page = 1;

            int _offset = (page - 1) * _perPage;
            var bookCount = _dataContext.BookArticles.Count();


            try
            {
                var bookList = await _dataContext.BookArticles
                .Include(a => a.Author_id)
                .Include(u => u.User_id)
                .Include(g => g.Genre_id)
                .Include(s => s.SubGenre_id)
                .Skip(_offset)
                .Take(_perPage)
                .ToListAsync();

                List<BookTransfer> listTransfer = new List<BookTransfer>();
                bookList.ForEach(book =>
                {
                    var booKTransfer = new BookTransfer()
                    {
                        Id = book.Id,
                        User_name = book.User_id.Login,
                        Genre_name = book.Genre_id.GenreName,
                        SubGenre_name = book.SubGenre_id.SubGenreName,
                        Author = book.Author_id!,
                        TitleImgPath = book.TitleImgPath,
                        Title = book.Title,
                        Description = book.Description,
                        FilePath = book.FilePath,
                        CreatedDate = book.CreatedDate,
                        Rating = book.Rating,
                    };
                    listTransfer.Add(booKTransfer);

                });

                return Ok(new { listTransfer, bookCount });
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally { _dataContext.Dispose(); }
        }

        // GET api/<BookController>/
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var bookDB = await _dataContext.BookArticles.Where(item => item.Id == Guid.Parse(id))
                     .Include(a => a.Author_id)
                     .Include(u => u.User_id)
                     .Include(g => g.Genre_id)
                     .Include(s => s.SubGenre_id).FirstOrDefaultAsync();

            if (bookDB != null)
            {
                var booKTransfer = new BookTransfer()
                {
                    Id = bookDB.Id,
                    User_name = bookDB.User_id.Login,
                    Genre_name = bookDB.Genre_id.GenreName,
                    SubGenre_name = bookDB.SubGenre_id.SubGenreName,
                    Author = bookDB.Author_id!,
                    TitleImgPath = bookDB.TitleImgPath,
                    Title = bookDB.Title,
                    Description = bookDB.Description,
                    FilePath = bookDB.FilePath,
                    CreatedDate = bookDB.CreatedDate,
                    Rating = bookDB.Rating,
                };
                return Ok(booKTransfer);
            }
            else return BadRequest("book не найдено");

        }
        [HttpGet]
        [Route("filterBySubGenre")]
        public async Task<ActionResult> Get(string subGenreId, int page) {
            if (page < 1) page = 1;

            int _offset = (page - 1) * _perPage;
            var bookCount = _dataContext.BookArticles
                .Where(item => item.SubGenre_id.Id == Guid
                                                   .Parse(subGenreId))
                .Count();

            try
            {
                var bookList = await _dataContext.BookArticles
                .Include(a => a.Author_id)
                .Include(u => u.User_id)
                .Include(g => g.Genre_id)
                .Include(s => s.SubGenre_id)
                .Where(item => item.SubGenre_id.Id == Guid.Parse(subGenreId))
                .Skip(_offset)
                .Take(_perPage)
                .ToListAsync();

                List<BookTransfer> listTransfer = new List<BookTransfer>();
                bookList.ForEach(book =>
                {
                    var booKTransfer = new BookTransfer()
                    {
                        Id = book.Id,
                        User_name = book.User_id.Login,
                        Genre_name = book.Genre_id.GenreName,
                        SubGenre_name = book.SubGenre_id.SubGenreName,
                        Author = book.Author_id!,
                        TitleImgPath = book.TitleImgPath,
                        Title = book.Title,
                        Description = book.Description,
                        FilePath = book.FilePath,
                        CreatedDate = book.CreatedDate,
                        Rating = book.Rating,
                    };
                    listTransfer.Add(booKTransfer);

                });

                return Ok(new { listTransfer, bookCount });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally { _dataContext.Dispose(); }

        }
        [HttpGet]
        [Route("bySearchSting")]
        public async Task<ActionResult> GetBysearch(string search) {
            if (search.Length == 0) return BadRequest("Server: search data is corrupted");
            try
            {
                var bookCollection = await _dataContext.BookArticles
                    .Include(i => i.Author_id)
                     .Include(u => u.User_id)
                     .Include(g => g.Genre_id)
                     .Include(s => s.SubGenre_id)
                                    .Where(item => EF.Functions.Like(item.Title, $"%{search}%") ||
                                    EF.Functions.Like(item.Author_id!.LastName, $"%{search}%") ||
                                    EF.Functions.Like(item.Author_id!.Name, $"%{search}%")
                                    )
                     .ToListAsync();    

                return Ok(bookCollection);
            }catch(Exception ex)
            {
                return BadRequest("Server: " + ex.Message);
            }
        } 
        
        // POST api/<BookController>
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Post(IFormCollection value)
        {
            try
            {
                var tittle = value["tittle"];
                var userId = value["userId"];
                var description = value["description"];
                var genre = value["genre"];
                var subGenre = value["subGenre"];
                var bookExt = value["extensionBook"];
                var coverExt = value["extensionTitleImg"];
                var authorLastName = value["authorLastName"];
                var authorName = value["authorName"];
                //   List<System.IO.File> bookFileList = value["bookFileList"];

              string bookFileName = $"{tittle}-{authorLastName} {authorName}".ToLower();


                if (value.Files.Count > 0)
                {
                    
                    foreach (var file in value.Files)
                        {
                         await _fileWriter.BookFileWriter(file, bookFileName.Trim());
                        }
                }
                var bookPath = Path.Combine("Resources", "Book", bookFileName);
                //var bookPath = await _fileWriter.BookFileWriter(value["bookFileList"].ToString(), tittle.ToString(), bookExt.ToString());
                var coverPath = await _fileWriter.CoverFileWriter(value["titleImgFile"].ToString(), tittle.ToString(), coverExt.ToString());

                if (string.IsNullOrEmpty(tittle) 
                    || string.IsNullOrEmpty(description) 
                    ||string.IsNullOrEmpty(genre) 
                    || string.IsNullOrEmpty(subGenre)
                    ||string.IsNullOrEmpty(bookPath) 
                    || string.IsNullOrEmpty(coverPath)) return BadRequest();

                Authors? initAuthor = null;

                if (await _dataContext.Authors
                            .AnyAsync(item => item.LastName == authorLastName.ToString()
                                                              && item.Name == authorName.ToString()))
                {
                    initAuthor = await _dataContext.Authors.Where
                           (item => item.LastName == authorLastName.ToString()
                               && item.Name == authorName.ToString()).FirstOrDefaultAsync();
                }
                if (initAuthor == null)
                {
                    initAuthor = new Authors()
                    {
                        Id = Guid.NewGuid(),
                        LastName = authorLastName.ToString().Trim(),
                        Name = authorName.ToString().Trim(),
                    };
                    await _dataContext.Authors.AddAsync(initAuthor);
                }

                var book = new BookArticle()
                {
                    Id = Guid.NewGuid(),
                    Genre_id = await _dataContext.Genres.Where(item => item.Id == Guid.Parse(genre.ToString())).FirstAsync() ,
                    SubGenre_id = await _dataContext.SubGenres.Where(item => item.Id == Guid.Parse(subGenre.ToString())).FirstAsync(),
                    User_id = await _dataContext.Users.Where(item => item.Id == Guid.Parse(userId.ToString())).FirstAsync(),
                    
                    TitleImgPath = coverPath.Trim(),
                    Title = tittle.ToString().Trim(),
                    Description = description.ToString().Trim(),
                    FilePath = bookPath.Trim(),
                    CreatedDate = DateTime.Now,
                    Author_id = initAuthor,
                    Rating = 0
                };
               await _dataContext.BookArticles.AddAsync(book);
               await _dataContext.SaveChangesAsync();

                BookTransfer bookTransfer = new BookTransfer
                {
                    Id = book.Id,
                    User_name = book.User_id.Login,
                    Genre_name =  book.Genre_id.GenreName,
                    SubGenre_name = book.SubGenre_id.SubGenreName,
                    Author = book.Author_id,
                    TitleImgPath = book.TitleImgPath,
                    Title = book.Title,
                    Description = book.Description,
                    FilePath = book.FilePath,
                    CreatedDate = book.CreatedDate,
                    Rating = book.Rating,
                };

                return Ok(bookTransfer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
