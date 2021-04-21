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

  [Route("api/director")]
  [ApiController]
  public class DirectorController : BaseController<DirectorController>
  {
    public DirectorController(ILogger<DirectorController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    /// <summary>
    /// Returns all directors. You can pass parameters to handle page and result items.
    /// Defaults to 20 items per page.
    /// </summary>
    /// <param name="itemsPerPage"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<ActionResult<List<DirectorDto>>> GetDirectors(int itemsPerPage = 20, int page = 1)
    {
      // We calculate how many items we shall skip to get next page (page offset).
      var toSkip = itemsPerPage * (page - 1);

      // We prepare the query without executing it.
      var directorsQuery = Context.Directors
        .TagWith("Retrieving all directors")
        .OrderBy(x => x.Id);

      // We calculate how many directors are in database.
      var totalDirectors = await directorsQuery.CountAsync();

      // If page provided doesn't exist we return bad request.
      if (page > ((totalDirectors / itemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      // We create the paged query request.
      var pagedDirectors = await directorsQuery
        .Skip(toSkip)
        .Take(itemsPerPage)
        .ToListAsync();

      // We create the result, which is paged.
      var result = new PagedResult<DirectorDto>
      {
        Results = pagedDirectors.Select(x => Mapper.Map<Director, DirectorDto>(x)).ToList(),
        Page = page,
        TotalPages = (totalDirectors / itemsPerPage) + 1,
        TotalElements = totalDirectors,
      };

      return Ok(result);
    }

    /// <summary>
    /// Returns a Director provided an ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public ActionResult<DirectorDto> GetDirector(long id)
    {
      // Returns the first director found with the spcefic ID. If not found, the result is null.
      var director = Context.Directors.Where(x => x.Id == id).SingleOrDefault();

      // If we haven't found a Director we return a not found response.
      if (director == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Director), id);
        return NotFound($"No {nameof(Director)} with Id {id} found in database");
      }

      Logger.LogInformation(LogTemplates.RequestEntity, nameof(Director), id);

      // Everything went ok, we map the found director to a dto and return it to client.
      return Ok(Mapper.Map<Director, DirectorDto>(director));
    }

    /// <summary>
    /// Adds a Director provided the necessary information.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<ActionResult<DirectorDto>> AddDirector([FromBody] AddDirectorDto dto)
    {
      // We map the provided director dto to an actual Director model.
      var director = Mapper.Map<AddDirectorDto, Director>(dto);

      // We add the director in the database.
      Context.Directors.Add(director);

      // And we save it
      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.CreatedEntity, nameof(Director), director);

      // We return the url where the entity was created.
      return CreatedAtAction(nameof(GetDirector), new { id = director.Id }, Mapper.Map<Director, DirectorDto>(director));
    }

    /// <summary>
    /// We delete a user provided an ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("")]
    public async Task<ActionResult> DeleteDirector(int id)
    {
      // We search for the director if it exists in our db.
      var director = Context.Directors.Where(x => x.Id == id).SingleOrDefault();

      // If not, we return a not found response.
      if (director == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Director), id);
        return NotFound("No director found in the database");
      }

      // We remove the director if there is one.
      Context.Directors.Remove(director);

      // We save the changes in our db.
      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Deleted, nameof(Director), id);

      // An empty success response.
      return NoContent();
    }

    /// <summary>
    /// We update a Director provided all the necessary information. Id is required.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<DirectorDto>> UpdateDirector(int id, AddDirectorDto dto)
    {
      // We search for the director to update
      var director = Context.Directors.Where(x => x.Id == id).SingleOrDefault();

      // If there is not director found we return a not found response.
      if (director == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Director), id);
        return NotFound($"No {nameof(Director)} with Id {id} found in database");
      }

      // Otherwise, we expicitly update the found director with newer values.
      director.FirstName = dto.FirstName;
      director.LastName = dto.LastName;
      director.Bio = dto.Bio;
      director.BirthDate = dto.BirthDate;

      // And we just save the changes to commit on previous updates.
      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Updated, nameof(Director), director);

      // We map the actual Director entity to a dto and return the updated director to the client.
      return Ok(Mapper.Map<Director, DirectorDto>(director));
    }
  }
}
