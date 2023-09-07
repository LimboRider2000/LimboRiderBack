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

                if(path == null) return BadRequest("Путь к книге в базе данных не найден");

                var tempPath = Path.ChangeExtension(Path.Combine(Directory.GetCurrentDirectory(), path), extension); // формируем путь к файлу 

                if (System.IO.File.Exists(tempPath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(tempPath); 

                    var strArr = tempPath.Split('\\');        
                    
                    var fileNameBytes = Encoding.UTF8.GetBytes(strArr[strArr.Length - 1]); // передаем имя файла с расширением 

                    var encodedFileName = Convert.ToBase64String(fileNameBytes);  // нужно для отображения кириллицы в имени файла 
                    
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
        [HttpGet]
        [Route("ExistFile")]
        public ActionResult ExistFile(string path) {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), path + ".pdf");
            if (System.IO.File.Exists(filePath))
            {
                return Ok(true);
            }
            return BadRequest("Файл для чтения не найден");   
        }


    }
}
