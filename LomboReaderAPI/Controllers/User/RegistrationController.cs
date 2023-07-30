using DevOpseTest.Services.Hash;
using DevOpseTest.Services.KDF;

using LimboReaderAPI.Data;
using LimboReaderAPI.Model.User;

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

        public RegistrationController(DataContext dataContext, IKDFService kDFService)
        {
            _dataContext = dataContext;
            _kDFService = kDFService;
        }


        // POST api/<RegistrationController>
        [HttpPost]
        public async IActionResult Post([FromBody] UserFormModel user)
        {
            try
            {
                if (user != null)
                {
                    var idStr = Guid.NewGuid().ToString().ToLower();
                    var id = Guid.Parse(idStr);

                    string dirivedKey = _kDFService.GetDirivedKey(user.password, idStr);

                    var newUser = new Data.Entety.User()
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
                    await _dataContext.Users.AddAsync(newUser)

                    return Ok(new { success = true });
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
