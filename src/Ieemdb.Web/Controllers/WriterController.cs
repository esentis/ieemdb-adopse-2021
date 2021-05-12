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
  using Esentis.Ieemdb.Web.Models.SearchCriteria;

  using Kritikos.PureMap.Contracts;

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
    /// <returns>List of <see cref="WriterDto"/>.</returns>
    [HttpPost("all")]
    public async Task<ActionResult<List<WriterDto>>> GetWriters(PaginationCriteria criteria)
    {
      var toSkip = criteria.ItemsPerPage * (criteria.Page - 1);

      var writersQuery = Context.Writers
        .TagWith("Retrieving all writers")
        .OrderBy(x => x.Id);

      var totalWriters = await writersQuery.CountAsync();

      if (criteria.Page > ((totalWriters / criteria.ItemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedWriters = await writersQuery
        .Skip(toSkip)
        .Take(criteria.ItemsPerPage)
        .ToListAsync();

      var result = new PagedResult<WriterDto>
      {
        Results = pagedWriters.Select(x => Mapper.Map<Writer, WriterDto>(x)).ToList(),
        Page = criteria.Page,
        TotalPages = (totalWriters / criteria.ItemsPerPage) + 1,
        TotalElements = totalWriters,
      };

      return Ok(result);
    }

    /// <summary>
    /// Returns a single Writer.
    /// </summary>
    /// <param name="id">Writer's unique ID.</param>
    /// <response code="200">Returns a single writer.</response>
    /// <response code="404">Writer not found.</response>
    /// <returns>Single <see cref="WriterDto"/>.</returns>
    [HttpGet("{id}")]
    public ActionResult<WriterDto> GetWriter(long id)
    {
      var writer = Context.Writers.SingleOrDefault(x => x.Id == id);

      if (writer == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Writer), id);
        return NotFound($"No {nameof(Writer)} with Id {id} found in database");
      }

      Logger.LogInformation(LogTemplates.RequestEntity, nameof(Writer), id);

      return Ok(Mapper.Map<Writer, WriterDto>(writer));
    }

    /// <summary>
    /// Adds a Writer.
    /// </summary>
    /// <param name="dto">Writer information.</param>
    /// <response code="201">Successfully added.</response>
    /// <returns>Created <see cref="WriterDto"/>.</returns>
    [HttpPost("")]
    public async Task<ActionResult<WriterDto>> AddWriter([FromBody] AddWriterDto dto)
    {
      var writer = Mapper.Map<AddWriterDto, Writer>(dto);

      Context.Writers.Add(writer);

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.CreatedEntity, nameof(Writer), writer);

      return CreatedAtAction(nameof(GetWriter), new { id = writer.Id }, Mapper.Map<Writer, WriterDto>(writer));
    }

    /// <summary>
    /// Deletes a Writer.
    /// </summary>
    /// <param name="id">Writer's unique ID.</param>
    /// <response code="204">Successfully deleted.</response>
    /// <response code="404">Writer not found.</response>
    [HttpDelete("")]
    public async Task<ActionResult> DeleteWriter(int id)
    {
      var writer = Context.Writers.SingleOrDefault(x => x.Id == id);

      if (writer == null || writer.IsDeleted)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Writer), id);
        return NotFound("No writer found in the database");
      }

      writer.IsDeleted = true;

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Deleted, nameof(Writer), id);

      return NoContent();
    }

    /// <summary>
    /// Updates a Writer.
    /// </summary>
    /// <param name="id">Writer's unique ID.</param>
    /// <param name="dto">Writer's information.</param>
    /// <returns>Updated <see cref="WriterDto"/>.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<WriterDto>> UpdateWriter(int id, AddWriterDto dto)
    {
      var writer = Context.Writers.SingleOrDefault(x => x.Id == id);

      if (writer == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Writer), id);
        return NotFound($"No {nameof(Writer)} with Id {id} found in database");
      }

      writer.FirstName = dto.FirstName;
      writer.LastName = dto.LastName;
      writer.Bio = dto.Bio;
      writer.BirthDate = dto.BirthDate;

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Updated, nameof(Writer), writer);

      return Ok(Mapper.Map<Writer, WriterDto>(writer));
    }
  }
}
