
using LomboReaderAPI.Data;
using LomboReaderAPI.Data.Entety;
using LomboReaderAPI.Model.Genre;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NuGet.Protocol.Core.Types;

using System.Threading.Tasks.Dataflow;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LomboReaderAPI.Controllers.Genre
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
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
