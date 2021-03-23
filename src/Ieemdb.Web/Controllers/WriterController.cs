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
      // We calculate how many items we shall skip to get next page (page offset).
      var toSkip = itemsPerPage * (page - 1);

      // We prepare the query without executing it.
      var writersQuery = Context.Writers
        .TagWith("Retrieving all writers")
        .OrderBy(x => x.Id);

      // We calculate how many writers are in database.
      var totalWriters = await writersQuery.CountAsync();

      // If page provided doesn't exist we return bad request.
      if (page > ((totalWriters / itemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      // We create the paged query request.
      var pagedWriters = await writersQuery
        .Skip(toSkip)
        .Take(itemsPerPage)
        .ToListAsync();

      // We create the result, which is paged.
      var result = new PagedResult<WriterDto>
      {
        Results = pagedWriters.Select(x => Mapper.Map<Writer, WriterDto>(x)).ToList(),
        Page = page,
        TotalPages = (totalWriters / itemsPerPage) + 1,
        TotalElements = totalWriters,
      };

      // We return OK and the paged results;
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
      // Returns the first writer found with the spcefic ID. If not found, the result is null.
      var writer = Context.Writers.Where(x => x.Id == id).SingleOrDefault();

      // If we haven't found a writer we return a not found response.
#pragma warning disable IDE0046 // Waiting for the new C# version
      if (writer == null)
#pragma warning restore IDE0046 // Convert to conditional expression
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Writer), id);
        return NotFound($"No {nameof(Writer)} with Id {id} found in database");
      }

      Logger.LogInformation(LogTemplates.RequestEntity, nameof(Writer), id);

      // Everything went ok, we map the found writer to a dto and return it to client.
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
      // We map the provided writer dto to an actual Writer model.
      var writer = Mapper.Map<AddWriterDto, Writer>(dto);

      // We add the writer in the database.
      Context.Writers.Add(writer);

      // And we save it
      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.CreatedEntity, nameof(Writer), writer);

      // We return the url where the entity was created.
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
      // We search for the writer if it exists in our db.
      var writer = Context.Writers.Where(x => x.Id == id).SingleOrDefault();

      // If not, we return a not found response.
      if (writer == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Writer), id);
        return NotFound("No writer found in the database");
      }

      // We remove the writer if there is one.
      Context.Writers.Remove(writer);

      // We save the changes in our db.
      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Deleted, nameof(Writer), id);

      // An empty success response.
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
      // We search for the writer to update
      var writer = Context.Writers.Where(x => x.Id == id).SingleOrDefault();

      // If there is not writer found we return a not found response.
      if (writer == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Writer), id);
        return NotFound($"No {nameof(Writer)} with Id {id} found in database");
      }

      // Otherwise, we expicitly update the found writer with newer values.
      writer.Name = dto.Name;
      writer.Bio = dto.bio;
      writer.BirthDate = dto.birthDate;

      // And we just save the changes to commit on previous updates.
      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Updated, nameof(Writer), writer);

      // We map the actual Writer entity to a dto and return the updated writer to the client.
      return Ok(Mapper.Map<Writer, WriterDto>(writer));
    }
  }
}
