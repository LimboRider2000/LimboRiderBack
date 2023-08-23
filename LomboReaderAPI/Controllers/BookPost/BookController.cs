using LimboReaderAPI.Data;
using LimboReaderAPI.Data.Entety;

using LomboReaderAPI.Model.Book;
using LomboReaderAPI.Services.File;

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;

using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LomboReaderAPI.Controllers.BookPost
{
    [Route("api/Book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        IFileWriter _filWriter;
        DataContext _dataContext;
        public BookController(IFileWriter filWriter, DataContext dataContext)
        {
            _filWriter = filWriter;
            _dataContext = dataContext;
        }

        // GET: api/<BookController>
        [HttpGet]
        public async Task<ActionResult> Get()
       {
            try
            {
                    var bookList = await _dataContext.BookArticles
                    .Include(a => a.Author_id)
                    .Include(u => u.User_id)
                    .Include(g => g.Genre_id)
                    .Include(s => s.SubGenre_id)
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

                return Ok(listTransfer);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally { _dataContext.Dispose(); }
       }

        // GET api/<BookController>/
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var bookDB = await _dataContext.BookArticles.Where(item=> item.Id == Guid.Parse(id))
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
                         await _filWriter.BookFileWriter(file, bookFileName);
                        }
                }
                var bookPath = Path.Combine("Resources", "Book", bookFileName);
                //var bookPath = await _filWriter.BookFileWriter(value["bookFileList"].ToString(), tittle.ToString(), bookExt.ToString());
                var coverPath = await _filWriter.CoverFileWriter(value["titleImgFile"].ToString(), tittle.ToString(), coverExt.ToString());

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
                        LastName = authorLastName.ToString(),
                        Name = authorName.ToString(),
                    };
                    await _dataContext.Authors.AddAsync(initAuthor);
                }

                var book = new BookArticle()
                {
                    Id = Guid.NewGuid(),
                    Genre_id = await _dataContext.Genres.Where(item => item.Id == Guid.Parse(genre.ToString())).FirstAsync() ,
                    SubGenre_id = await _dataContext.SubGenres.Where(item => item.Id == Guid.Parse(subGenre.ToString())).FirstAsync(),
                    User_id = await _dataContext.Users.Where(item => item.Id == Guid.Parse(userId.ToString())).FirstAsync(),
                    
                    TitleImgPath = coverPath,
                    Title = tittle.ToString(),
                    Description = description.ToString(),
                    FilePath = bookPath,
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
