using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Net.Http.Headers;

namespace LomboReaderAPI.Controllers.File
{
    [Route("api/avatarFile")]
    [ApiController]
    public class AvatarfileController : ControllerBase
    {
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Post()
        {
            try
            {
                var user = Request.HttpContext.User;

                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToserv = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToserv, fileName);
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
    }
}
