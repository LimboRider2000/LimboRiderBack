using Humanizer.Localisation;

using LimboReaderAPI.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace LimboReaderAPI.Controllers.File
{
    [Route("api/File")]
    [ApiController]
    public class AvatarFileController : ControllerBase
    {
        private DataContext _dataContext;
        private string? fullPath { get; set; }
        private string? fileName { get; set; }

        public AvatarFileController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Post()
        {
            try
            {
                var user = Request.HttpContext.User;

                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToServer = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition)
                                    ?.FileName?.Trim('"');
                    if (fileName == null) return BadRequest(); ;
                    var fullPath = Path.Combine(pathToServer, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using var fs = new FileStream(fullPath, FileMode.Create);
                    file.CopyTo(fs);

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server error: {ex}");
            }

        }

        [HttpGet, DisableRequestSizeLimit]
         public async Task<ActionResult> GetBookFile(string book_id,string extension) {
            try
            {
                string? path = await _dataContext.BookArticles
                        .Where(item => item.Id == Guid.Parse(book_id))
                        .Select(item => item.FilePath)
                        .FirstOrDefaultAsync();

                if(path == null) return BadRequest("Файл книги не найден");

                string baseDirectory = Directory.GetCurrentDirectory();

                 fullPath = Path.Combine(baseDirectory, path);

                var tempPath = fullPath;
                tempPath = Path.ChangeExtension(tempPath, extension);


                if (System.IO.File.Exists(tempPath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(tempPath);

                    var strArr = path.Split('\\');

                    fileName = strArr[strArr.Length - 1];

                    strArr = fileName.Split('.');

                    var fileNameBytes = Encoding.UTF8.GetBytes(fileName);
                    var encodedFileName = Convert.ToBase64String(fileNameBytes);

                    var contentDisposition = new System.Net.Mime.ContentDisposition
                    {
                        FileName = WebUtility.UrlEncode(encodedFileName),
                        Inline = false
                    };

                    Response.Headers.Add("Content-Disposition", contentDisposition.ToString());


                    return File(fileBytes, "application/octet-stream");
                }
                else return BadRequest("Файл с таким расширением не найден попробуйте другой формат");


            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        


    }
}
