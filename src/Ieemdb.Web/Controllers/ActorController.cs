namespace Esentis.Ieemdb.Web.Controllers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;
  using Esentis.Ieemdb.Web.Models.SearchCriteria;

  using Kritikos.Extensions.Linq;
  using Kritikos.PureMap;
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
    /// Returns all Actors.
    /// </summary>
    /// <param name="criteria">Paging criteria.</param>
    /// <returns>List of <see cref="ActorDto"/>.</returns>
    [HttpPost("all")]
    public async Task<ActionResult<List<ActorDto>>> GetActors(PaginationCriteria criteria,
      CancellationToken token = default)
    {
      var toSkip = criteria.ItemsPerPage * (criteria.Page - 1);

      var actorsQuery = Context.Actors
        .TagWith("Retrieving all actors")
        .OrderBy(x => x.Id);

      var totalActors = await actorsQuery.CountAsync(token);

      if (criteria.Page > ((totalActors / criteria.ItemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedActors = await actorsQuery
        .Skip(toSkip)
        .Take(criteria.ItemsPerPage)
        .ToListAsync(token);

      var result = new PagedResult<ActorDto>
      {
        Results = pagedActors.Select(x => Mapper.Map<Actor, ActorDto>(x)).ToList(),
        Page = criteria.Page,
        TotalPages = (totalActors / criteria.ItemsPerPage) + 1,
        TotalElements = totalActors,
      };

      return Ok(result);
    }

    /// <summary>
    /// Searches for an Actor.
    /// </summary>
    /// <param name="query">Search term.</param>
    /// <param name="criteria">Paging criteria.</param>
    /// <returns>List of <see cref="ActorDto"/>.</returns>
    [HttpPost("search")]
    public async Task<ActionResult<List<ActorDto>>> Search(
      PersonSearchCriteria criteria,
      CancellationToken token = default)
    {
      var actorsQuery = Context.Actors
        .TagWith($"Searching for {criteria.Query}")
        .FullTextSearchIf(string.IsNullOrWhiteSpace(criteria.Query), criteria.Query)
        .OrderBy(x => x.Id);

      var totalActors = await actorsQuery.CountAsync(token);

      if (criteria.Page > ((totalActors / criteria.ItemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedActors = await actorsQuery.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Actor, ActorDto>(Mapper)
        .ToListAsync(token);

      var result = new PagedResult<ActorDto>
      {
        Results = pagedActors,
        Page = criteria.Page,
        TotalPages = (totalActors / criteria.ItemsPerPage) + 1,
        TotalElements = totalActors,
      };

      return Ok(result);
    }

    /// <summary>
    /// Returns a single Actor.
    /// </summary>
    /// <param name="id">Actor's ID.</param>
    /// <response code="200">Success returns single Actor.</response>
    /// <response code="400">Actor was not found.</response>
    /// <returns>Single <see cref="ActorDto"/>.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ActorDto>> GetActor(long id, CancellationToken token = default)
    {
      var actor = await Context.Actors.SingleOrDefaultAsync(x => x.Id == id, token);

      if (actor == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Actor), id);
        return NotFound($"No {nameof(Actor)} with Id {id} found in database");
      }

      Logger.LogInformation(LogTemplates.RequestEntity, nameof(Actor), id);

      return Ok(Mapper.Map<Actor, ActorDto>(actor));
    }

    /// <summary>
    /// Adds an Actor.
    /// </summary>
    /// <param name="dto">Actor information.</param>
    /// <response code="201">Actor successfully added.</response>
    /// <returns>Created <see cref="ActorDto"/>.</returns>
    [HttpPost("")]
    public async Task<ActionResult<ActorDto>> AddActor([FromBody] AddActorDto dto, CancellationToken token = default)
    {
      var actor = Mapper.Map<AddActorDto, Actor>(dto);

      Context.Actors.Add(actor);

      await Context.SaveChangesAsync(token);
      Logger.LogInformation(LogTemplates.CreatedEntity, nameof(Actor), actor);

      return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, Mapper.Map<Actor, ActorDto>(actor));
    }

    /// <summary>
    /// Deletes an Actor.
    /// </summary>
    /// <param name="id">Actor's unique ID.</param>
    /// <response code="204">Deleted successfully.</response>
    /// <response code="404">Actor not found.</response>
    [HttpDelete("")]
    public async Task<ActionResult> DeleteActor(int id, CancellationToken token = default)
    {
      var actor = await Context.Actors.SingleOrDefaultAsync(x => x.Id == id, token);

      if (actor == null || actor.IsDeleted)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Actor), id);
        return NotFound("No actor found in the database");
      }

      actor.IsDeleted = true;

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Deleted, nameof(Actor), id);

      return NoContent();
    }

    /// <summary>
    /// Updates an Actor.
    /// </summary>
    /// <param name="id">Actor's unique ID.</param>
    /// <param name="dto">Actor's information.</param>
    /// <returns>Updated <see cref="ActorDto"/>.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ActorDto>> UpdateActor(int id, AddActorDto dto, CancellationToken token = default)
    {
      var actor = await Context.Actors.SingleOrDefaultAsync(x => x.Id == id, token);

      if (actor == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Actor), id);
        return NotFound($"No {nameof(Actor)} with Id {id} found in database");
      }

      actor.FullName = dto.FullName;
      actor.Bio = dto.Bio;
      actor.BirthDate = dto.BirthDate;

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Updated, nameof(Actor), actor);

      return Ok(Mapper.Map<Actor, ActorDto>(actor));
    }
  }
}
