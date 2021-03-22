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

  [Route("api/actor")]
  [ApiController]
  public class ActorController : BaseController<ActorController>
  {
    public ActorController(ILogger<ActorController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    /// <summary>
    /// Returns all actors. You can pass parameters to handle page and result items.
    /// Defaults to 20 items per page.
    /// </summary>
    /// <param name="itemsPerPage"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<ActionResult<List<ActorDto>>> GetActors(int itemsPerPage = 20, int page = 1)
    {
      // We calculate how many items we shall skip to get next page (page offset).
      var toSkip = itemsPerPage * (page - 1);

      // We prepare the query without executing it.
      var actorsQuery = Context.Actors
        .TagWith("Retrieving all actors")
        .OrderBy(x => x.Id);

      // We calculate how many actors are in database.
      var totalActors = await actorsQuery.CountAsync();

      // If page provided doesn't exist we return bad request.
      if (page > ((totalActors / itemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      // We create the paged query request.
      var pagedActors = await actorsQuery
        .Skip(toSkip)
        .Take(itemsPerPage)
        .ToListAsync();

      // We create the result, which is paged.
      var result = new PagedResult<ActorDto>
      {
        Results = pagedActors.Select(x => Mapper.Map<Actor, ActorDto>(x)).ToList(),
        Page = page,
        TotalPages = (totalActors / itemsPerPage) + 1,
        TotalElements = totalActors,
      };

      // We return OK and the paged results;
      return Ok(result);
    }

    /// <summary>
    /// Returns an actor provided an ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public ActionResult<ActorDto> GetActor(long id)
    {
      // Returns the first actor found with the spcefic ID. If not found, the result is null.
      var actor = Context.Actors.Where(x => x.Id == id).SingleOrDefault();

      // If we haven't found an actor we return a not found response.
#pragma warning disable IDE0046 // Waiting for the new C# version
      if (actor == null)
#pragma warning restore IDE0046 // Convert to conditional expression
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Actor), id);
        return NotFound($"No {nameof(Actor)} with Id {id} found in database");
      }

      Logger.LogInformation(LogTemplates.RequestEntity, nameof(Actor), id);

      // Everything went ok, we map the found actor to a dto and return it to client.
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
      // We map the provided actor dto to an actual Actor model.
      var actor = Mapper.Map<AddActorDto, Actor>(dto);

      // We add the actor in the database.
      Context.Actors.Add(actor);

      // And we save it
      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.CreatedEntity, nameof(Actor), actor);

      // We return the url where the entity was created.
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
      // We search for the actor if it exists in our db.
      var actor = Context.Actors.Where(x => x.Id == id).SingleOrDefault();

      // If not, we return a not found response.
      if (actor == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Actor), id);
        return NotFound("No actor found in the database");
      }

      // We remove the actor if there is one.
      Context.Actors.Remove(actor);

      // We save the changes in our db.
      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Deleted, nameof(Actor), id);

      // An empty success response.
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
      // We search for the actor to update
      var actor = Context.Actors.Where(x => x.Id == id).SingleOrDefault();

      // If there is not actor found we return a not found response.
      if (actor == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Actor), id);
        return NotFound($"No {nameof(Actor)} with Id {id} found in database");
      }

      // Otherwise, we expicitly update the found actor with newer values.
      actor.Name = dto.Name;
      actor.Bio = dto.bio;
      actor.BirthDate = dto.birthDate;

      // And we just save the changes to commit on previous updates.
      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Updated, nameof(Actor), actor);

      // We map the actual Actor entity to a dto and return the updated actor to the client.
      return Ok(Mapper.Map<Actor, ActorDto>(actor));
    }
  }
}
