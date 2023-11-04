using DevOpseTest.Services.KDF;

using LimboReaderAPI.Data;

using LimboReaderAPI.Model.User;
using LimboReaderAPI.Services.CodeGenerator;
using LimboReaderAPI.Services.File;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LimboReaderAPI.Controllers.User
{
    [Route("api/User")]
    [ApiController]
    public class MyUserController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IFileWriter _fileWriter;
        private readonly IKDFService KDF;
        public MyUserController(DataContext dataContext, ICodeGenerator codeGenerator, IKDFService kDF, IFileWriter fileWriter)
        {
            _dataContext = dataContext;
            _codeGenerator = codeGenerator;
             KDF = kDF;
            _fileWriter = fileWriter;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var userList = await _dataContext.Users.ToListAsync();

                if (userList.Count < 0 || userList == null) return BadRequest("Нет пользователей");

                return Ok(userList);

            }catch(Exception ex) {
                return BadRequest(ex.Message);  
            }
        }

        // GET api/<UserController>/5
        [HttpGet]
        [Route("userCount")]
        public ActionResult GetUserCount()
        {
            return Ok(new {count = _dataContext.Users.Count()});
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        [HttpPut]
        [Route("passwordEdit")]
        public ActionResult Put(IFormCollection formData) {
            try
            {
                string oldPasword = formData["oldPassword"].ToString();
                if (oldPasword == "") throw new ArgumentNullException("passwordEdit -- oldPasword is null");
                string newPassword = formData["newPassword"].ToString();
                if (newPassword == "") throw new ArgumentNullException("passwordEdit -- newPassword is null");
                Guid userId = Guid.Parse(formData["userId"].ToString());


                var currentUser = _dataContext.Users.Where(u => u.Id == userId).FirstOrDefault();
                if (currentUser == null) return BadRequest();

                var oldPassHashChack = KDF.GetDirivedKey(oldPasword, userId.ToString());

                if (oldPassHashChack != currentUser.PasswordHash) return Ok(new { isSuccess = false });

                currentUser.PasswordHash = KDF.GetDirivedKey(newPassword, userId.ToString());    

                _dataContext.SaveChanges(); 

                return Ok(new {isSuccess = true});
                //"6D86270CF3E314F82D5A5C03E9A09A9BA3B7F226"
            }
            catch(Exception) {
                return BadRequest();   
            } 
        }
        // PUT api/<UserController>/5
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UserEditFormModel data)
        {
            try
            {
                if (data != null) {
                    var user = await _dataContext.Users.Where(item => item.Id == data.Id).FirstOrDefaultAsync();
                    if(user == null) return BadRequest("Редактирование пользователя: Проблемы с id");

                    if (user.Email != data.Email) {
                        user.ActivateCode = _codeGenerator.RandomCodeGen();
                    }
                    user.Login = data.Login;   
                    user.Name = data.Name;
                    user.Email = data.Email;
                    user.Active = data.Active;
                    user.UserRole = data.UserRole;

                    await _dataContext.SaveChangesAsync();
                    return Ok(user);    
                }
                return BadRequest("Редактирование пользователя: Проблемы с данными");
            }
            catch(Exception ex) {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut]
        [Route("editUserSelf")]
        public ActionResult PutUserSelf(IFormCollection formData) {
            try
            {
                Guid userId = Guid.Parse(formData["userId"].ToString());
                string name = formData["name"].ToString();
                if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("PutUserSelf -- name is null");
                string mail = formData["mail"].ToString();
                if (String.IsNullOrEmpty(mail)) throw new ArgumentNullException("PutUserSelf -- mail is null");
                string login = formData["login"].ToString();
                if (String.IsNullOrEmpty(login)) throw new ArgumentNullException("PutUserSelf -- login is null");

                Data.Entety.User? user = _dataContext.Users.Where(u => u.Id == userId).FirstOrDefault();
                if (user == null)
                {
                    throw new ArgumentNullException("PutUserSelf -- user not find by id");
                }

                if (formData.Files.Count > 0)
                {
                   user.Avatar = _fileWriter.AvatarFileChange(formData.Files[0], userId.ToString(), user.Avatar);
                };

                if (!user.Email.Equals(mail))
                {
                    if (_dataContext.Users.Any(u => u.Email == mail))
                    {
                        return Ok(new { isSuccess = false, message = "Ошибка почта уже есть зарегистрированная" });
                    }

                    user.ActivateCode = _codeGenerator.RandomCodeGen();
                    user.Email = mail;
                }

                if (!user.Login.Equals(login)) {
                    if (_dataContext.Users.Any(u => u.Login == login)) {
                        return Ok(new { isSuccess = false, message = "Логин занят" });
                    } 
                    user.Login = login;
                }
                user.Name = name;   

                _dataContext.SaveChanges();
               
                return Ok(new { isSuccess = true,user = user} );
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest();

            }catch (Exception ex) {

                return BadRequest();
            }

        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}") ]
        public void Delete(int id)
        {
        }
    }
}
