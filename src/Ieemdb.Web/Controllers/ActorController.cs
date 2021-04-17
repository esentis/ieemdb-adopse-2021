namespace Esentis.Ieemdb.Web.Controllers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Route("api/actor")]
  [ApiController]
  public class ActorController : BaseController<ActorController>
  {
    public ActorController(ILogger<ActorController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    /// <summary>
    /// Returns all actors. You can pass parameters to handle page and result count.
    /// </summary>
    /// <param name="itemsPerPage">Define how many items shall be returned. </param>
    /// <param name="page">Choose which page of the results shall be returned.</param>
    /// <returns>Returns a list of Actors.</returns>
    [HttpGet("")]
    public async Task<ActionResult<List<ActorDto>>> GetActors(int itemsPerPage = 20, int page = 1)
    {
      var toSkip = itemsPerPage * (page - 1);

      var actorsQuery = Context.Actors
        .TagWith("Retrieving all actors")
        .OrderBy(x => x.Id);

      var totalActors = await actorsQuery.CountAsync();

      if (page > ((totalActors / itemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedActors = await actorsQuery
        .Skip(toSkip)
        .Take(itemsPerPage)
        .ToListAsync();

      var result = new PagedResult<ActorDto>
      {
        Results = pagedActors.Select(x => Mapper.Map<Actor, ActorDto>(x)).ToList(),
        Page = page,
        TotalPages = (totalActors / itemsPerPage) + 1,
        TotalElements = totalActors,
      };

      return Ok(result);
    }

    /// <summary>
    /// Searches for an actor provided a text string.
    /// </summary>
    /// <param name="query">Search term.</param>
    /// <param name="itemsPerPage">Define how many items shall be returned. </param>
    /// <param name="page">Choose which page of the results shall be returned.</param>
    /// <returns>Returns a list of Actors that match the text string.</returns>
    [HttpGet("search")]
    public async Task<ActionResult<List<ActorDto>>> Search(string query, int itemsPerPage = 20, int page = 1)
    {
      var toSkip = itemsPerPage * (page - 1);

      var actorsQuery = Context.Actors
        .TagWith($"Searching for {query}")
        .FullTextSearch(query)
        .OrderBy(x => x.Id);

      var totalActors = await actorsQuery.CountAsync();

      if (page > ((totalActors / itemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedActors = await actorsQuery
        .Skip(toSkip)
        .Take(itemsPerPage)
        .ToListAsync();

      var result = new PagedResult<ActorDto>
      {
        Results = pagedActors.Select(x => Mapper.Map<Actor, ActorDto>(x)).ToList(),
        Page = page,
        TotalPages = (totalActors / itemsPerPage) + 1,
        TotalElements = totalActors,
      };

      return Ok(result);
    }

    /// <summary>
    /// Returns an actor provided an ID.
    /// </summary>
    /// <param name="id">Actor's ID</param>
    /// <returns>One single Actor.</returns>
    /// <response code="400">Actor was not found.</response>
    [HttpGet("{id}")]
    public ActionResult<ActorDto> GetActor(long id)
    {
      var actor = Context.Actors.SingleOrDefault(x => x.Id == id);

      if (actor == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Actor), id);
        return NotFound($"No {nameof(Actor)} with Id {id} found in database");
      }

      Logger.LogInformation(LogTemplates.RequestEntity, nameof(Actor), id);

      return Ok(Mapper.Map<Actor, ActorDto>(actor));
    }

    /// <summary>
    /// Adds an actor provided the necessary information.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<ActionResult<ActorDto>> AddActor([FromBody] AddActorDto dto)
    {
      var actor = Mapper.Map<AddActorDto, Actor>(dto);

      Context.Actors.Add(actor);

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.CreatedEntity, nameof(Actor), actor);

      return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, Mapper.Map<Actor, ActorDto>(actor));
    }

    /// <summary>
    /// We delete a user provided an ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("")]
    public async Task<ActionResult> DeleteActor(int id)
    {
      var actor = Context.Actors.SingleOrDefault(x => x.Id == id);

      if (actor == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Actor), id);
        return NotFound("No actor found in the database");
      }

      Context.Actors.Remove(actor);

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Deleted, nameof(Actor), id);

      return NoContent();
    }

    /// <summary>
    /// We update an Actor provided all the necessary information. Id is required.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ActorDto>> UpdateActor(int id, AddActorDto dto)
    {
      var actor = Context.Actors.SingleOrDefault(x => x.Id == id);

      if (actor == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Actor), id);
        return NotFound($"No {nameof(Actor)} with Id {id} found in database");
      }

      actor.Name = dto.Name;
      actor.Bio = dto.Bio;
      actor.BirthDate = dto.BirthDate;

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Updated, nameof(Actor), actor);

      return Ok(Mapper.Map<Actor, ActorDto>(actor));
    }
  }
}
