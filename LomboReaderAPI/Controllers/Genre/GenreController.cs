using LimboReaderAPI.Data;
using LimboReaderAPI.Data.Entety;
using LimboReaderAPI.Model.Genre;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NuGet.Protocol.Core.Types;

using System.Threading.Tasks.Dataflow;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LimboReaderAPI.Controllers.Genre
{
    [Route("api/Genre")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private DataContext _dataContext;

        public GenreController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string genreName)
        {
           if (String.IsNullOrEmpty(genreName)) return BadRequest("Не коректное имя") ;
            if (_dataContext.Genres.Any(item => item.GenreName == genreName))
                return BadRequest("Такой жанр уже есть");

           var newGenre = new Data.Entety.Genre()
           { Id = Guid.NewGuid(), GenreName = genreName };
            try
            {
                await _dataContext.AddAsync(newGenre);
                await _dataContext.SaveChangesAsync();
                return Ok(newGenre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public  async Task<ActionResult> GetAll()
        {
            try
            {
                List<Data.Entety.Genre> genreList = await _dataContext.Genres.ToListAsync();
                List<Data.Entety.SubGenre> subGenresList = await _dataContext.SubGenres.ToListAsync();

                if (genreList == null|| genreList.Count == 0) return Ok();

                List<GenreAndSubgenreCoverForView> listGenre = new List<GenreAndSubgenreCoverForView>();

                genreList.ForEach(item => listGenre.Add(new GenreAndSubgenreCoverForView() { Genre = item }));

                if (subGenresList == null || subGenresList.Count == 0 ) return Ok(listGenre);

                foreach (var genre in listGenre)
                {
                    foreach (var subGenre in subGenresList)
                    {
                        if (genre.Genre.Id == subGenre.Genre_id)
                        {
                            genre.SubGenreList.Add(subGenre);
                        }
                    }
                }
                return Ok(listGenre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);   
            }
          
        }
        [HttpGet]
        [Route("addSubGenre")] 
        public async Task<ActionResult> AddSubGenre([FromQuery] string genre_id, string subGenreName) {
            if (string.IsNullOrEmpty(genre_id) || string.IsNullOrEmpty(subGenreName))
                return BadRequest("Недостаточно данных: проверти запрос.");
            if (await _dataContext.SubGenres.
                AnyAsync(item =>
                item.SubGenreName == subGenreName
                && item.Genre_id == Guid.Parse(genre_id)))
                return BadRequest("Такой поджанр уже существует");
            try
            {
                var newSubGenre = new SubGenre()
                {
                    Id = Guid.NewGuid(),
                    Genre_id = Guid.Parse(genre_id),
                    SubGenreName = subGenreName
                };

                await _dataContext.SubGenres.AddAsync(newSubGenre);
                await _dataContext.SaveChangesAsync();
                return Ok(newSubGenre);
            }catch (Exception ex) {
                return BadRequest("Ошибка сервера: " + ex.Message);
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        public async  Task<ActionResult> Put([FromBody] Data.Entety.Genre value)
        {
            try
            {
                if (value != null)
                {
                    Data.Entety.Genre? genre =  await _dataContext.Genres.Where(item=> item.Id == value.Id).FirstOrDefaultAsync();
                    if (genre != null)
                    {
                        genre.GenreName = value.GenreName;
                        await _dataContext.SaveChangesAsync();
                    return Ok("true");
                    }
                    return BadRequest("Жанра с таким Id не существует");
                }
                else
                    return BadRequest("Данные повреждены");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        [HttpPut]
        [Route("putSubGenre")]
        public async Task<ActionResult> Put([FromBody] Data.Entety.SubGenre value)
        {
            try
            {
                if (value != null)
                {
                    Data.Entety.SubGenre? subGenre = await _dataContext.SubGenres.Where(item => item.Id == value.Id).FirstOrDefaultAsync();
                    if (subGenre != null)
                    {
                        subGenre.SubGenreName = value.SubGenreName;
                        await _dataContext.SaveChangesAsync();
                        return Ok("true");
                    }
                    return BadRequest("Поджанра с таким Id не существует");
                }
                else
                    return BadRequest("Данные повреждены");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery]string id)
        {
            try
            {
                if (id != null)
                { 
                    var pasrseId = Guid.Parse(id);

                    var subGenreList =
                        await _dataContext.SubGenres
                              .Where(item => item.Genre_id == pasrseId).ToListAsync();
                    subGenreList.ForEach(item => _dataContext.SubGenres.Remove(item));

                    _dataContext.Genres.Remove(
                        await _dataContext.Genres.
                                 Where(item => item.Id == pasrseId)
                                     .FirstAsync()
                        );
                    await _dataContext.SaveChangesAsync();
                    return Ok("true");
                }
                else return BadRequest("При удалении произошла ошибка");
            }catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteSubGenre")]
        public async Task<ActionResult> DeleteSubGenre([FromQuery] string id)
        {
            if (id != null) {
                try
                {
                    _dataContext.SubGenres.Remove(
                           await _dataContext.SubGenres.
                                    Where(item => item.Id == Guid.Parse(id))
                                        .FirstAsync()
                           );
                    await _dataContext.SaveChangesAsync();
                    return Ok("true");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);  
                }
            }
            else return BadRequest();
        }
    }
}
