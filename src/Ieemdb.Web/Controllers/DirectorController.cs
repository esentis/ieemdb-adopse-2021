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

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Authorize]
  [Route("api/director")]
  [ApiController]
  public class DirectorController : BaseController<DirectorController>
  {
    public DirectorController(ILogger<DirectorController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    /// <summary>
    /// Returns all Directors.
    /// </summary>
    /// <param name="criteria">Pagination criteria.</param>
    /// <response code="200">Returns the directors.</response>
    /// <response code="400">Page doesn't exist.</response>
    /// <returns>A list of <see cref="PersonDto"/>.</returns>
    [AllowAnonymous]
    [HttpPost("all")]
    public async Task<ActionResult<List<PersonDto>>> GetDirectors(
      PaginationCriteria criteria,
      CancellationToken token = default)
    {
      var directorsQuery = Context.People.Where(p => p.KnownFor == DepartmentEnums.Directing)
        .TagWith("Retrieving all directors")
        .OrderBy(x => x.Id);

      var totalDirectors = await directorsQuery.CountAsync(token);

      if (criteria.Page > ((totalDirectors / criteria.ItemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedDirectors = await directorsQuery.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Person, PersonDto>(Mapper)
        .ToListAsync(token);

      var result = new PagedResult<PersonDto>
      {
        Results = pagedDirectors,
        Page = criteria.Page,
        TotalPages = (totalDirectors / criteria.ItemsPerPage) + 1,
        TotalElements = totalDirectors,
      };

      return Ok(result);
    }

    /// <summary>
    /// Searches for a Director.
    /// </summary>
    /// <param name="criteria">Search criteria.</param>
    /// <response code="200">Returns found Actors.</response>
    /// <response code="400">Page doesn't exist.</response>
    /// <returns>List of <see cref="PersonDto"/>.</returns>
    [AllowAnonymous]
    [HttpPost("search")]
    public async Task<ActionResult<List<PersonDto>>> Search(
      PersonSearchCriteria criteria,
      CancellationToken token = default)
    {
      var directorsQuery = Context.People.Where(p => p.KnownFor == DepartmentEnums.Directing)
        .TagWith($"Searching for {criteria.Query}")
        .FullTextSearchIf(string.IsNullOrWhiteSpace(criteria.Query), criteria.Query)
        .OrderBy(x => x.Id);

      var totalDirectors = await directorsQuery.CountAsync(token);

      if (criteria.Page > ((totalDirectors / criteria.ItemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedDirectors = await directorsQuery.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Person, PersonDto>(Mapper)
        .ToListAsync(token);

      var result = new PagedResult<PersonDto>
      {
        Results = pagedDirectors,
        Page = criteria.Page,
        TotalPages = (totalDirectors / criteria.ItemsPerPage) + 1,
        TotalElements = totalDirectors,
      };

      return Ok(result);
    }

    /// <summary>
    /// Returns a single Director.
    /// </summary>
    /// <param name="id">Director's unique ID.</param>
    /// <response code="200">Returns single director.</response>
    /// <response code="404">Director not found.</response>
    /// <returns>A single <see cref="PersonDto"/>.</returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDto>> GetDirector(long id, CancellationToken token = default)
    {
      var director = await Context.People.Where(p => (p.KnownFor == DepartmentEnums.Directing) && p.Id == id).Project<Person, PersonDto>(Mapper).SingleOrDefaultAsync(token);

      if (director == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Person), id);
        return NotFound($"No {nameof(Person)} with Id {id} found in database");
      }

      Logger.LogInformation(LogTemplates.RequestEntity, nameof(Person), id);

      return Ok(director);
    }

    /// <summary>
    /// Adds a Director.
    /// </summary>
    /// <param name="dto">Director information.</param>
    /// <response code="201">Successfully added director.</response>
    /// <response code="401">Unauthorized.</response>
    /// <returns>Created <see cref="PersonDto"/>.</returns>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost("")]
    public async Task<ActionResult<PersonDto>> AddDirector([FromBody] AddPersonDto dto)
    {
      var director = Mapper.Map<AddPersonDto, Person>(dto);
      director.KnownFor = DepartmentEnums.Directing;

      Context.People.Add(director);

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.CreatedEntity, nameof(Person), director);

      return CreatedAtAction(nameof(GetDirector), new { id = director.Id }, Mapper.Map<Person, PersonDto>(director));
    }

    /// <summary>
    /// Removes a Director.
    /// </summary>
    /// <param name="id">Director's unique ID.</param>
    /// <response code="201">Successfully deleted.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Director not found.</response>
    /// <returns>No content.</returns>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpDelete("")]
    public async Task<ActionResult> DeleteDirector(int id, CancellationToken token = default)
    {
      var director = await Context.People.Where(p => p.KnownFor == DepartmentEnums.Directing).Where(x => x.Id == id).SingleOrDefaultAsync(token);

      if (director == null || director.IsDeleted)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Person), id);
        return NotFound("No director found in the database");
      }

      director.IsDeleted = true;

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Deleted, nameof(Person), id);

      return NoContent();
    }

    /// <summary>
    /// Updates a Director.
    /// </summary>
    /// <param name="id">Director's unique ID.</param>
    /// <param name="dto">Director's information.</param>
    /// <response code="200">Director successfully updated.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Director not found.</response>
    /// <returns>Updated <see cref="PersonDto"/>.</returns>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPut("{id}")]
    public async Task<ActionResult<PersonDto>> UpdateDirector(int id, AddPersonDto dto, CancellationToken token = default)
    {
      var director = await Context.People.Where(p => p.KnownFor == DepartmentEnums.Directing).Where(x => x.Id == id).Project<Person,PersonDto>(Mapper).SingleOrDefaultAsync(token);

      if (director == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Person), id);
        return NotFound($"No {nameof(Person)} with Id {id} found in database");
      }

      director.FullName = dto.FullName;
      director.Bio = dto.Bio;
      director.BirthDay = dto.BirthDate;
      director.DeathDay = dto.DeathDate;
      director.Image = dto.Image;

      await Context.SaveChangesAsync(token);
      Logger.LogInformation(LogTemplates.Updated, nameof(Person), director);

      return Ok(director);
    }
  }
}
