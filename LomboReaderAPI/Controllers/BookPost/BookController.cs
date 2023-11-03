using LimboReaderAPI.Data;
using LimboReaderAPI.Data.Entety;

using LimboReaderAPI.Model.Book;
using LimboReaderAPI.Services.File;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;

using NuGet.Common;

using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LimboReaderAPI.Controllers.BookPost
{
    [Route("api/Book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IFileWriter _fileWriter;
        private DataContext _dataContext;
        private int _perPage = 5;
        private Dictionary<string, bool> fileExtensionMap = new Dictionary<string, bool>() {
                    { "pdf", false },
                    {"epub",false },
                    {"fb2",false }
                };
        public BookController(IFileWriter fileWriter, DataContext dataContext)
        {
            _fileWriter = fileWriter;
            _dataContext = dataContext;
        }

        // GET: api/<BookController>
        [HttpGet]
        public async Task<ActionResult> Get(int page = 1)
        {
            try
            {
                if (page < 1) page = 1;

                int _offset = (page - 1) * _perPage;
                var bookCount = _dataContext.BookArticles.Count();

                


                var bookList = await _dataContext.BookArticles
                .Include(a => a.Author_id)
                .Include(u => u.User_id)
                .Include(g => g.Genre_id)
                .Include(s => s.SubGenre_id)
                .OrderByDescending(t=> t.CreatedDate)
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
                                    .Where(item =>
                                    EF.Functions.Like(item.Title, $"%{search}%") ||
                                    EF.Functions.Like(item.Author_id!.LastName, $"%{search}%") ||
                                    EF.Functions.Like(item.Author_id!.Name, $"%{search}%") ||
                                    EF.Functions.Like(item.Author_id!.LastName + " " + item.Author_id!.Name, $"%{search}%") ||
                                    EF.Functions.Like(item.Author_id!.Name + " " + item.Author_id!.LastName, $"%{search}%") ||
                                    EF.Functions.Like(item.Author_id!.Name + " " + item.Author_id!.LastName + " " + item.Title, $"%{search}%") ||
                                    EF.Functions.Like(item.Title + " " + item.Author_id!.Name + " " + item.Author_id!.LastName, $"%{search}%") ||
                                    EF.Functions.Like(item.User_id.Login, $"%{search}%")
                                    )
                     .ToListAsync();


                List<BookTransfer> listTransfer = new List<BookTransfer>();
                bookCollection.ForEach(book =>
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
            } catch (Exception ex)
            {
                return BadRequest("Server: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("getBookByUserId")]
        public async Task<ActionResult> GetBookByUserId(string userId)
        {
            try
            {
                var bookList = await _dataContext.BookArticles
                .Include(a => a.Author_id)
                .Include(u => u.User_id)
                .Include(g => g.Genre_id)
                .Include(s => s.SubGenre_id)
                .Where(item => item.User_id.Id == Guid.Parse(userId))
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
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally { _dataContext.Dispose(); }

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
                // var bookFileList = value["bookFileList"] as List<System.IO.File>;

                var bookId = Guid.NewGuid();
                string bookFileName = bookId.ToString();


                if (value.Files.Count > 0)
                {

                    foreach (var file in value.Files)
                    {
                        await _fileWriter.BookFileWriter(file, bookFileName);
                    }
                }
                var bookPath = Path.Combine("Resources", "Book", bookFileName);
                //var bookPath = await _fileWriter.BookFileWriter(value["bookFileList"].ToString(), tittle.ToString(), bookExt.ToString());
                var coverPath = await _fileWriter.CoverFileWriter(value["titleImgFile"].ToString(), bookId.ToString(), coverExt.ToString());

                if (string.IsNullOrEmpty(tittle)
                    || string.IsNullOrEmpty(description)
                    || string.IsNullOrEmpty(genre)
                    || string.IsNullOrEmpty(subGenre)
                    || string.IsNullOrEmpty(bookPath)
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
                    Id = bookId,
                    Genre_id = await _dataContext.Genres.Where(item => item.Id == Guid.Parse(genre.ToString())).FirstAsync(),
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
                    Genre_name = book.Genre_id.GenreName,
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
        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Put(IFormCollection formData) {
            try
            {
                var extensionTitleImg = formData["extensionTitleImg"];
                var authorId = formData["authorId"];
                var bookId = formData["idBook"];
                var titleImgFile = formData["titleImgFile"];
                var isTitleImgChange = Convert.ToBoolean(formData["isTitleImgChange"]);
                var authorLastName = formData["authorLastName"];
                var authorName = formData["authorName"];
                var title = formData["title"];
                var description = formData["description"];
                var genre = formData["genre"];
                var subGenre = formData["subGenre"];

                var currentBook = _dataContext
                                  .BookArticles
                                  .Where(
                                   item =>
                                   item.Id == Guid.Parse(bookId.ToString()))
                                   .FirstOrDefault();

                if (currentBook == null)
                    return BadRequest("Книга не найдена");

                if (isTitleImgChange)
                {
                    currentBook.TitleImgPath = await _fileWriter.EditCover(titleImgFile.ToString(), bookId.ToString()
                          , currentBook.TitleImgPath, extensionTitleImg.ToString());
                }


                foreach (var file in formData.Files)
                {
                    var ext = file.FileName.Trim().Substring(file.FileName.LastIndexOf(".") + 1);

                    var fileName = currentBook.FilePath.Substring(currentBook.FilePath.LastIndexOf("\\") + 1);

                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), (fileName + "." + ext));

                    if (System.IO.File.Exists(pathToFile))
                    {
                        _fileWriter.DeleteBookFile(pathToFile);
                    }

                    await _fileWriter.BookFileWriter(file, fileName);
                }

                var currentAuthor = await _dataContext.Authors
                    .Where(author => author.Id == Guid.Parse(authorId.ToString())).FirstOrDefaultAsync();
                if (currentAuthor != null)
                {
                    currentAuthor.LastName = authorLastName.ToString();
                    currentAuthor.Name = authorName.ToString();
                }

                // путь к файлу обложки 
                currentBook.Title = title.ToString();
                currentBook.Description = description.ToString();

                currentBook.Genre_id = await _dataContext.Genres.Where(item => item.Id == Guid.Parse(genre.ToString())).FirstAsync();
                currentBook.SubGenre_id = await _dataContext.SubGenres.Where(item => item.Id == Guid.Parse(subGenre.ToString())).FirstAsync();


                 _dataContext.SaveChanges();

                return Ok();
            }
            catch(Exception ex) {
                return BadRequest(ex.Message);            
            }
        }








        // DELETE api/<BookController>/5
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            try {
                _dataContext.BookArticles.Where(b => b.Id == Guid.Parse(id)).ExecuteDelete();
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest();
            }

        }
    }
}
