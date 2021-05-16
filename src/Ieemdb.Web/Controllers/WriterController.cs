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

  [Route("api/writer")]
  [ApiController]
  public class WriterController : BaseController<WriterController>
  {
    public WriterController(ILogger<WriterController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    /// <summary>
    /// Returns all Writers.
    /// </summary>
    /// <param name="criteria">Pagination criteria.</param>
    /// <response code="200">Returns all writers.</response>
    /// <response code="400">Page doesn't exist.</response>
    /// <returns>List of <see cref="PersonDto"/>.</returns>
    [HttpPost("all")]
    public async Task<ActionResult<List<PersonDto>>> GetWriters(
      PaginationCriteria criteria,
      CancellationToken token = default)
    {
      var writersQuery = Context.People.Where(p=>p.KnownFor==DepartmentEnums.Writing)
        .TagWith("Retrieving all writers")
        .OrderBy(x => x.Id);

      var totalWriters = await writersQuery.CountAsync(token);

      if (criteria.Page > ((totalWriters / criteria.ItemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var count = await writersQuery.CountAsync(token);

      var pagedWriters = await writersQuery.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Person, PersonDto>(Mapper)
        .ToListAsync(token);

      PagedResult<PersonDto> results = new()
      {
        Page = criteria.Page,
        Results = pagedWriters,
        TotalElements = count,
        TotalPages = (count / criteria.ItemsPerPage) + 1,
      };

      return Ok(results);
    }

    /// <summary>
    /// Returns a single Writer.
    /// </summary>
    /// <param name="id">Writer's unique ID.</param>
    /// <response code="200">Returns a single writer.</response>
    /// <response code="404">Writer not found.</response>
    /// <returns>Single <see cref="PersonDto"/>.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDto>> GetWriter(long id, CancellationToken token = default)
    {
      var writer = await Context.People.Where(p => p.KnownFor == DepartmentEnums.Writing)
        .SingleOrDefaultAsync(x => x.Id == id, token);

      if (writer == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Person), id);
        return NotFound($"No {nameof(Person)} with Id {id} found in database");
      }

      Logger.LogInformation(LogTemplates.RequestEntity, nameof(Person), id);

      return Ok(Mapper.Map<Person, PersonDto>(writer));
    }

    /// <summary>
    /// Searches for a Writer.
    /// </summary>
    /// <param name="criteria">Search criteria.</param>
    /// <response code="200">Returns found Writers.</response>
    /// <response code="400">Page doesn't exist.</response>
    /// <returns>List of <see cref="PersonDto"/>.</returns>
    [HttpPost("search")]
    public async Task<ActionResult<List<PersonDto>>> Search(
      PersonSearchCriteria criteria,
      CancellationToken token = default)
    {
      var writersQuery = Context.People.Where(p => p.KnownFor == DepartmentEnums.Writing)
        .TagWith($"Searching for {criteria.Query}")
        .FullTextSearchIf(string.IsNullOrWhiteSpace(criteria.Query), criteria.Query)
        .OrderBy(x => x.Id);

      var totalWriters = await writersQuery.CountAsync(token);

      if (criteria.Page > ((totalWriters / criteria.ItemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedWriters = await writersQuery.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Person, PersonDto>(Mapper)
        .ToListAsync(token);

      var result = new PagedResult<PersonDto>
      {
        Results = pagedWriters,
        Page = criteria.Page,
        TotalPages = (totalWriters / criteria.ItemsPerPage) + 1,
        TotalElements = totalWriters,
      };

      return Ok(result);
    }

    /// <summary>
    /// Adds a Writer.
    /// </summary>
    /// <param name="dto">Writer information.</param>
    /// <response code="201">Successfully added.</response>
    /// <response code="401">Unauthorized.</response>
    /// <returns>Created <see cref="PersonDto"/>.</returns>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost("")]
    public async Task<ActionResult<PersonDto>> AddWriter([FromBody] AddPersonDto dto, CancellationToken token = default)
    {
      var writer = Mapper.Map<AddPersonDto, Person>(dto);
      writer.KnownFor = DepartmentEnums.Writing;

      Context.People.Add(writer);

      await Context.SaveChangesAsync(token);
      Logger.LogInformation(LogTemplates.CreatedEntity, nameof(Person), writer);

      return CreatedAtAction(nameof(GetWriter), new { id = writer.Id }, Mapper.Map<Person, PersonDto>(writer));
    }

    /// <summary>
    /// Deletes a Writer.
    /// </summary>
    /// <param name="id">Writer's unique ID.</param>
    /// <response code="204">Successfully deleted.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Writer not found.</response>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpDelete("")]
    public async Task<ActionResult> DeleteWriter(int id, CancellationToken token = default)
    {
      var writer = await Context.People.Where(p => p.KnownFor == DepartmentEnums.Writing)
        .SingleOrDefaultAsync(x => x.Id == id, token);

      if (writer == null || writer.IsDeleted)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Person), id);
        return NotFound("No writer found in the database");
      }

      writer.IsDeleted = true;

      await Context.SaveChangesAsync(token);
      Logger.LogInformation(LogTemplates.Deleted, nameof(Person), id);

      return NoContent();
    }

    /// <summary>
    /// Updates a Writer.
    /// </summary>
    /// <param name="id">Writer's unique ID.</param>
    /// <param name="dto">Writer's information.</param>
    /// <response code="204">Successfully updated.</response>
    /// <response code="401">Unauthorized.</response>
    /// <returns>Updated <see cref="PersonDto"/>.</returns>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPut("{id}")]
    public async Task<ActionResult<PersonDto>> UpdateWriter(int id, AddPersonDto dto, CancellationToken token = default)
    {
      var writer = await Context.People.Where(p => p.KnownFor == DepartmentEnums.Writing)
        .SingleOrDefaultAsync(x => x.Id == id, token);

      if (writer == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Person), id);
        return NotFound($"No {nameof(Person)} with Id {id} found in database");
      }

      writer.FullName = dto.FullName;
      writer.Bio = dto.Bio;
      writer.BirthDay = dto.BirthDate;
      writer.DeathDay = dto.DeathDate;
      writer.Image = dto.Image;

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Updated, nameof(Person), writer);

      return Ok(Mapper.Map<Person, PersonDto>(writer));
    }
  }
}
