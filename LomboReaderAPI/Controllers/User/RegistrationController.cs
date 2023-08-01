using DevOpseTest.Services.Hash;
using DevOpseTest.Services.KDF;

using LimboReaderAPI.Data;
using LimboReaderAPI.Model.User;

using LomboReaderAPI.Services.Mail;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Xml.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LimboReaderAPI.Controllers.UserController
{
    [Route("api/registration")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private DataContext _dataContext;
        private IKDFService _kDFService;
        private IMailService _mailService;
        private Data.Entety.User? newUser;

        public RegistrationController(DataContext dataContext, IKDFService kDFService,
            IMailService mailService)
        {
            _dataContext = dataContext;
            _kDFService = kDFService;
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string code,string userId) {
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(userId))
            {
                try
                {
                    var user = await _dataContext.Users
                         .Where(item => item.Id == Guid.Parse(userId))
                         .FirstOrDefaultAsync();
                    
                    if (user == null) return Ok(new
                    {
                        success = false,
                        message = "Пользователь с таким Id не найден"
                    });

                    _ = int.TryParse(user.ActivateCode, out int num1);
                    _ = int.TryParse(code, out int num2);
                    if (num1  != num2)
                        return Ok(new { success = false,message = "Не правильный код Активации" });

                    user.ActivateCode = null;
                    user.Active = true;

                    await _dataContext.SaveChangesAsync();
                    return Ok(new {success = true});
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }return BadRequest();
        }
        // POST api/<RegistrationController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserFormModel user)
        {
            try
            {
                if (user != null)
                {
                    var idStr = Guid.NewGuid().ToString().ToLower();
                    var id = Guid.Parse(idStr);

                    string dirivedKey = _kDFService.GetDirivedKey(user.password, idStr);

                    newUser = new Data.Entety.User()
                    {
                        Id = id,
                        Name = user.name,
                        Login = user.login,
                        Email = user.email,
                        PasswordHash = dirivedKey,
                        UserRole = "User",
                        Avatar = user.avatar,
                        RegisterDt = DateTime.Now,
                        ActivateCode = RandomCodeGen(),
                    };
                    await _dataContext.Users.AddAsync(newUser);
                    await _dataContext.SaveChangesAsync();

                    _mailService.SendMail(newUser.Email, newUser.ActivateCode);
                    
                    return Ok(new { success = true, id = idStr });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
         
        }
        
        string RandomCodeGen()
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < 7; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }




    }
}
