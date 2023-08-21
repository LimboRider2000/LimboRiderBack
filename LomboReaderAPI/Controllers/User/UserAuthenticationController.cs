using DevOpseTest.Services.KDF;

using LimboReaderAPI.Data;

using LomboReaderAPI.Model.User;
using LomboReaderAPI.Services.Mail;

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
        public IMailService _mailService;
        public UserAuthenticationController(DataContext dbContext, IKDFService kdfService, IMailService mailService)
        {
            _dataContext = dbContext;
            _kdfService = kdfService;
            _mailService = mailService;
        }

        // POST api/Auntification
        [HttpPost]
        public async Task<ActionResult> SingIn([FromBody] UserAuthModel user)
        {

            try
            {
                if (user != null)
                {

                    var currUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

                    if (currUser == null) return Ok(new { success = false });

                    String derivedKey = _kdfService.GetDirivedKey(
                       user.Password, currUser.Id.ToString());
                    if (derivedKey == currUser.PasswordHash)
                    {

                        if (currUser.ActivateCode != null) {
                            _mailService.SendMail(currUser.Email, currUser.ActivateCode);
                            return Ok(new { notVerifi = true, id = currUser.Id });
                        };

                        if (!currUser.Active) return Ok(new { notActive = true } );

                        return Ok(new { success = true, currUser,avatarPath = currUser.Avatar });
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
