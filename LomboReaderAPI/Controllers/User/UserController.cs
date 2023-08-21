using LimboReaderAPI.Data;

using LomboReaderAPI.Model.User;
using LomboReaderAPI.Services.CodeGenerator;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LimboReaderAPI.Controllers.User
{
    [Route("api/User")]
    [ApiController]
    public class MyUserController : ControllerBase
    {
        private DataContext _userDataContext;
        private ICodeGenerator _codeGenerator;

        public MyUserController(DataContext dataContext, ICodeGenerator codeGenerator)
        {
            _userDataContext = dataContext;
            _codeGenerator = codeGenerator;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var userList = await _userDataContext.Users.ToListAsync();

                if (userList.Count < 0 || userList == null) return BadRequest("Нет пользователей");

                return Ok(userList);

            }catch(Exception ex) {
                return BadRequest(ex.Message);  
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "data";
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UserEditFormModel data)
        {
            try
            {
                if (data != null) {
                    var user = await _userDataContext.Users.Where(item => item.Id == data.Id).FirstOrDefaultAsync();
                    if(user == null) return BadRequest("Редактирование пользователя: Проблемы с id");

                    if (user.Email != data.Email) {
                        user.ActivateCode = _codeGenerator.RandomCodeGen();
                    }
                    user.Login = data.Login;   
                    user.Name = data.Name;
                    user.Email = data.Email;
                    user.Active = data.Active;
                    user.UserRole = data.UserRole;

                    await _userDataContext.SaveChangesAsync();
                    return Ok(user);    
                }
                return BadRequest("Редактирование пользователя: Проблемы с данными");
            }
            catch(Exception ex) {
                return BadRequest(ex.Message);
            }

        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
