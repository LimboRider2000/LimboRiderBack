using LomboReaderAPI.Model.User;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LomboReaderAPI.Controllers.UserController
{
    [Route("api/registration")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {

        // POST api/<RegistrationController>
        [HttpPost]
        public IActionResult Post([FromBody] UserFormModel user)
        {
            if (user != null)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest();
            }
        }




    }
}
