using LimboReaderAPI.Data;
using LimboReaderAPI.Data.Entety;
using LimboReaderAPI.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LimboReaderAPI.Controllers.Comment
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        DataContext _dataContext;

        public CommentController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: api/<ValuesController>
        

        // GET api/<ValuesController>/5
        [HttpGet]
        public async Task<ActionResult> Get(string book_id)
        {
            try
            {
                var cv = await _dataContext.Comments.Join(_dataContext.Users,
                     c => c.User,
                     u => u.Id,
                     (c, u) => new
                     {
                         c.Id,
                         c.BookArticle_Id,
                         c.User,
                         c.DateTime,
                         c.Comment,
                         userObj = u
                     }).Where(item => item.BookArticle_Id == Guid.Parse(book_id)).ToListAsync();
                return Ok(cv);  
            }catch (Exception ex) {
                return BadRequest(ex.Message);
            }
                              
        }

        // POST api/<ValuesController>
        [HttpPost]
        public ActionResult Post(IFormCollection form)
        {
            if (form == null) return BadRequest("Server : форма не пришла");

            var newComment = new Data.Entety.Comments()
            {
                Id = Guid.NewGuid(),
                BookArticle_Id = Guid.Parse(form["book_id"].ToString()),
                User = Guid.Parse(form["user_id"].ToString()),
                Comment = form["comment"].ToString(),
                DateTime = DateTime.Now,
            };

            _dataContext.Comments.Add(newComment);
            _dataContext.SaveChanges();

            var user = _dataContext.Users
                             .Where(item => item.Id == newComment.User)
                             .FirstOrDefault();

            if(user == null) return BadRequest("Server : Проблемы с юзером");

            var newComentView = new CommentView()
            {
                BookArticle_Id = newComment.BookArticle_Id,
                User = newComment.User,
                Id = newComment.Id,
                Comment = newComment.Comment,
                DateTime = newComment.DateTime,
                userObj = user
            };
            
            return Ok(newComentView);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
               _dataContext.Comments.
                              Remove(_dataContext.Comments
                                     .Where(item => item.Id == Guid.Parse(id))
                                     .First());
                _dataContext.SaveChanges();
                return Ok();    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
