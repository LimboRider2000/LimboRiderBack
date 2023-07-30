using DevOpseTest.Services.KDF;

using LimboReaderAPI.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LimboReaderAPI.Controllers.UserController
{
    [Route("Api/Authentication")]
    [ApiController]
    public class UserAuthenticationController : ControllerBase
    {
        public DataContext _dataContext;
        public IKDFService _kdfService;
        public UserAuthenticationController(DataContext dbContext, IKDFService kdfService)
        {
            _dataContext = dbContext;
            _kdfService = kdfService;
        }

        // POST api/Auntification
        [HttpPost]
        public async Task<ActionResult> SingIn([FromForm] Data.Entety.User user)
        {

            var login = user.Login;
            var password = user.PasswordHash;
            try
            {
                if (!String.IsNullOrEmpty(login) && !String.IsNullOrEmpty(password))
                {

                    var currUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Login == login);
                    if (currUser == null) return Ok(new { success = false });

                    String derivedKey = _kdfService.GetDirivedKey(
                        password, currUser.Id.ToString());
                    if (derivedKey == currUser.PasswordHash)
                    {
                        return Ok(new { success = true, currUser });
                    }
                    
                }
                return Ok(new { success = false });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

    }
}
