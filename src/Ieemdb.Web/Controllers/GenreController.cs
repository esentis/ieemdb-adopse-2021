namespace Esentis.Ieemdb.Web.Controllers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Route("api/genre")]
  [ApiController]
  public class GenreController : BaseController<GenreController>
  {
    public GenreController(ILogger<GenreController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    /// <summary>
    /// Returns all genres. You can pass parameters to handle page and result items.
    /// Defaults to 20 items per page.
    /// </summary>
    /// <param name="itemsPerPage"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<ActionResult<List<GenreDto>>> GetGenres(int itemsPerPage = 20, int page = 1)
    {
      // We calculate how many items we shall skip to get next page (page offset).
      var toSkip = itemsPerPage * (page - 1);

      // We prepare the query without executing it.
      var genresQuery = Context.Genres
        .TagWith("Retrieving all genres")
        .OrderBy(x => x.Id);

      // We calculate how many genres are in database.
      var totalGenres = await genresQuery.CountAsync();

      // If page provided doesn't exist we return bad request.
      if (page > ((totalGenres / itemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      // We create the paged query request.
      var pagedGenres = await genresQuery
        .Skip(toSkip)
        .Take(itemsPerPage)
        .ToListAsync();

      // We create the result, which is paged.
      var result = new PagedResult<GenreDto>
      {
        Results = pagedGenres.Select(x => Mapper.Map<Genre, GenreDto>(x)).ToList(),
        Page = page,
        TotalPages = (totalGenres / itemsPerPage) + 1,
        TotalElements = totalGenres,
      };

      // We return OK and the paged results;
      return Ok(result);
    }

    /// <summary>
    /// Returns an genre provided an ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public ActionResult<GenreDto> GetGenre(long id)
    {
      // Returns the first genre found with the spcefic ID. If not found, the result is null.
      var genre = Context.Genres.Where(x => x.Id == id).SingleOrDefault();

      // If we haven't found an genre we return a not found response.
#pragma warning disable IDE0046 // Waiting for the new C# version
      if (genre == null)
#pragma warning restore IDE0046 // Convert to conditional expression
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Genre), id);
        return NotFound($"No {nameof(Genre)} with Id {id} found in database");
      }

      Logger.LogInformation(LogTemplates.RequestEntity, nameof(Genre), id);

      // Everything went ok, we map the found genre to a dto and return it to client.
      return Ok(Mapper.Map<Genre, GenreDto>(genre));
    }

    /// <summary>
    /// Adds an genre provided the necessary information.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<ActionResult<GenreDto>> AddGenre([FromBody] AddGenreDto dto)
    {
      // We map the provided genre dto to an actual Genre model.
      var genre = Mapper.Map<AddGenreDto, Genre>(dto);

      // We add the genre in the database.
      Context.Genres.Add(genre);

      // And we save it
      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.CreatedEntity, nameof(Genre), genre);

      // We return the url where the entity was created.
      return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, Mapper.Map<Genre, GenreDto>(genre));
    }

    /// <summary>
    /// We delete a user provided an ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("")]
    public async Task<ActionResult> DeleteGenre(int id)
    {
      // We search for the genre if it exists in our db.
      var genre = Context.Genres.Where(x => x.Id == id).SingleOrDefault();

      // If not, we return a not found response.
      if (genre == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Genre), id);
        return NotFound("No genre found in the database");
      }

      // We remove the genre if there is one.
      Context.Genres.Remove(genre);

      // We save the changes in our db.
      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Deleted, nameof(Genre), id);

      // An empty success response.
      return NoContent();
    }

    /// <summary>
    /// We update an Genre provided all the necessary information. Id is required.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<GenreDto>> UpdateGenre(int id, AddGenreDto dto)
    {
      // We search for the genre to update
      var genre = Context.Genres.Where(x => x.Id == id).SingleOrDefault();

      // If there is not genre found we return a not found response.
      if (genre == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Genre), id);
        return NotFound($"No {nameof(Genre)} with Id {id} found in database");
      }

      // Otherwise, we expicitly update the found genre with newer values.
      genre.Name = dto.Name;

      // And we just save the changes to commit on previous updates.
      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Updated, nameof(Genre), genre);

      // We map the actual Genre entity to a dto and return the updated genre to the client.
      return Ok(Mapper.Map<Genre, GenreDto>(genre));
    }
  }
}
