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
  using Esentis.Ieemdb.Web.Models.Enums;
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
    /// <response code="200">Returns list of Actors.</response>
    /// <response code="400">Page doesn't exist.</response>
    /// <returns>List of <see cref="PersonDto"/>.</returns>
    [HttpPost("all")]
    public async Task<ActionResult<List<PersonDto>>> GetActors(PaginationCriteria criteria,
      CancellationToken token = default)
    {
      var toSkip = criteria.ItemsPerPage * (criteria.Page - 1);

      var actorsQuery = Context.People.Where(p => p.KnownFor == DepartmentEnums.Acting)
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

      var result = new PagedResult<PersonDto>
      {
        Results = pagedActors.Select(x => Mapper.Map<Person, PersonDto>(x)).ToList(),
        Page = criteria.Page,
        TotalPages = (totalActors / criteria.ItemsPerPage) + 1,
        TotalElements = totalActors,
      };

      return Ok(result);
    }

    /// <summary>
    /// Searches for an Actor.
    /// </summary>
    /// <param name="criteria">Search criteria.</param>
    /// <response code="200">Returns found Actors.</response>
    /// <response code="400">Page doesn't exist.</response>
    /// <returns>List of <see cref="PersonDto"/>.</returns>
    [HttpPost("search")]
    public async Task<ActionResult<List<PersonDto>>> Search(
      PersonSearchCriteria criteria,
      CancellationToken token = default)
    {
      var actorsQuery = Context.People.Where(p => p.KnownFor == DepartmentEnums.Acting)
        .TagWith($"Searching for {criteria.Query}")
        .FullTextSearchIf(string.IsNullOrWhiteSpace(criteria.Query), criteria.Query)
        .OrderBy(x => x.Id);

      var totalActors = await actorsQuery.CountAsync(token);

      if (criteria.Page > ((totalActors / criteria.ItemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedActors = await actorsQuery.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Person, PersonDto>(Mapper)
        .ToListAsync(token);

      var result = new PagedResult<PersonDto>
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
    /// <response code="404">Actor was not found.</response>
    /// <returns>Single <see cref="PersonDto"/>.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDto>> GetActor(long id, CancellationToken token = default)
    {
      var actor = await Context.People.Where(p => p.KnownFor == DepartmentEnums.Acting)
        .SingleOrDefaultAsync(x => x.Id == id, token);

      if (actor == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Person), id);
        return NotFound($"No {nameof(Person)} with Id {id} found in database");
      }

      Logger.LogInformation(LogTemplates.RequestEntity, nameof(Person), id);

      return Ok(Mapper.Map<Person, PersonDto>(actor));
    }

    /// <summary>
    /// Adds an Actor.
    /// </summary>
    /// <param name="dto">Actor information.</param>
    /// <response code="201">Actor successfully added.</response>
    /// <returns>Created <see cref="PersonDto"/>.</returns>
    [HttpPost("")]
    public async Task<ActionResult<PersonDto>> AddActor([FromBody] AddPersonDto dto, CancellationToken token = default)
    {
      var actor = Mapper.Map<AddPersonDto, Person>(dto);

      Context.People.Add(actor);

      await Context.SaveChangesAsync(token);
      Logger.LogInformation(LogTemplates.CreatedEntity, nameof(Person), actor);

      return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, Mapper.Map<Person, PersonDto>(actor));
    }

    /// <summary>
    /// Deletes an Actor.
    /// </summary>
    /// <param name="id">Actor's unique ID.</param>
    /// <response code="204">Deleted successfully.</response>
    /// <response code="404">Actor not found.</response>
    /// <returns>No content.</returns>
    [HttpDelete("")]
    public async Task<ActionResult> DeleteActor(int id, CancellationToken token = default)
    {
      var actor = await Context.People.Where(p => p.KnownFor == DepartmentEnums.Acting)
        .SingleOrDefaultAsync(x => x.Id == id, token);

      if (actor == null || actor.IsDeleted)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Person), id);
        return NotFound("No actor found in the database");
      }

      actor.IsDeleted = true;

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Deleted, nameof(Person), id);

      return NoContent();
    }

    /// <summary>
    /// Updates an Actor.
    /// </summary>
    /// <param name="id">Actor's unique ID.</param>
    /// <param name="dto">Actor's information.</param>
    /// <response code="200">Returns updated Actor.</response>
    /// <response code="404">No actor found.</response>
    /// <returns>Updated <see cref="PersonDto"/>.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PersonDto>> UpdateActor(int id, AddPersonDto dto, CancellationToken token = default)
    {
      var actor = await Context.People.Where(p => p.KnownFor == DepartmentEnums.Acting)
        .SingleOrDefaultAsync(x => x.Id == id, token);

      if (actor == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Person), id);
        return NotFound($"No {nameof(Person)} with Id {id} found in database");
      }

      actor.FullName = dto.FullName;
      actor.Bio = dto.Bio;
      actor.BirthDay = dto.BirthDate;
      actor.DeathDay = dto.DeathDate;
      actor.Image = dto.Image;

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Updated, nameof(Person), actor);

      return Ok(Mapper.Map<Person, PersonDto>(actor));
    }
  }
}
