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

  [Route("api/writer")]
  [ApiController]
  public class WriterController : BaseController<WriterController>
  {
    public WriterController(ILogger<WriterController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    /// <summary>
    /// Returns all writers. You can pass parameters to handle page and result items.
    /// Defaults to 20 items per page.
    /// </summary>
    /// <param name="itemsPerPage"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<ActionResult<List<WriterDto>>> GetWriters(int itemsPerPage = 20, int page = 1)
    {
      var toSkip = itemsPerPage * (page - 1);

      var writersQuery = Context.Writers
        .TagWith("Retrieving all writers")
        .OrderBy(x => x.Id);

      var totalWriters = await writersQuery.CountAsync();

      if (page > ((totalWriters / itemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedWriters = await writersQuery
        .Skip(toSkip)
        .Take(itemsPerPage)
        .ToListAsync();

      var result = new PagedResult<WriterDto>
      {
        Results = pagedWriters.Select(x => Mapper.Map<Writer, WriterDto>(x)).ToList(),
        Page = page,
        TotalPages = (totalWriters / itemsPerPage) + 1,
        TotalElements = totalWriters,
      };

      return Ok(result);
    }

    /// <summary>
    /// Returns a writer provided an ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
    /// Adds a writer provided the necessary information.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
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
    /// We delete a user provided an ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("")]
    public async Task<ActionResult> DeleteWriter(int id)
    {
      var writer = Context.Writers.SingleOrDefault(x => x.Id == id);

      if (writer == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Writer), id);
        return NotFound("No writer found in the database");
      }

      Context.Writers.Remove(writer);

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Deleted, nameof(Writer), id);

      return NoContent();
    }

    /// <summary>
    /// We update a writer provided all the necessary information. Id is required.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
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
