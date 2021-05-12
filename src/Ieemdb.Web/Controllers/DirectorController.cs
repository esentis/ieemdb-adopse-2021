namespace Esentis.Ieemdb.Web.Controllers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
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
    /// <returns>A list of <see cref="DirectorDto"/>.</returns>
    [HttpPost("all")]
    public async Task<ActionResult<List<DirectorDto>>> GetDirectors(
      PersonSearchCriteria criteria,
      CancellationToken token = default)
    {
      var directorsQuery = Context.Directors
        .TagWith("Retrieving all directors")
        .OrderBy(x => x.Id);

      var totalDirectors = await directorsQuery.CountAsync(token);

      if (criteria.Page > ((totalDirectors / criteria.ItemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedDirectors = await directorsQuery.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Director, DirectorDto>(Mapper)
        .ToListAsync(token);

      var result = new PagedResult<DirectorDto>
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
    /// <returns>A single <see cref="DirectorDto"/>.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<DirectorDto>> GetDirector(long id, CancellationToken token = default)
    {
      var director = await Context.Directors.Where(x => x.Id == id).Project<Director,DirectorDto>(Mapper).SingleOrDefaultAsync(token);

      if (director == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Director), id);
        return NotFound($"No {nameof(Director)} with Id {id} found in database");
      }

      Logger.LogInformation(LogTemplates.RequestEntity, nameof(Director), id);

      return Ok(director);
    }

    /// <summary>
    /// Adds a Director.
    /// </summary>
    /// <param name="dto">Director information.</param>
    /// <response code="201">Successfully added director.</response>
    /// <returns>Created <see cref="DirectorDto"/>.</returns>
    [HttpPost("")]
    public async Task<ActionResult<DirectorDto>> AddDirector([FromBody] AddDirectorDto dto)
    {
      var director = Mapper.Map<AddDirectorDto, Director>(dto);

      Context.Directors.Add(director);

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.CreatedEntity, nameof(Director), director);

      return CreatedAtAction(nameof(GetDirector), new { id = director.Id },
        Mapper.Map<Director, DirectorDto>(director));
    }

    /// <summary>
    /// Removes a Director.
    /// </summary>
    /// <param name="id">Director's unique ID.</param>
    /// <response code="201">Successfully deleted.</response>
    /// <response code="404">Director not found.</response>
    [HttpDelete("")]
    public async Task<ActionResult> DeleteDirector(int id, CancellationToken token = default)
    {
      var director = await Context.Directors.Where(x => x.Id == id).SingleOrDefaultAsync(token);

      if (director == null || director.IsDeleted)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Director), id);
        return NotFound("No director found in the database");
      }

      director.IsDeleted = true;

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Deleted, nameof(Director), id);

      return NoContent();
    }

    /// <summary>
    /// Updates a Director.
    /// </summary>
    /// <param name="id">Director's unique ID.</param>
    /// <param name="dto">Director's information.</param>
    /// <response code="200">Director successfully updated.</response>
    /// <response code="404">Director not found.</response>
    /// <returns>Updated <see cref="DirectorDto"/>.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<DirectorDto>> UpdateDirector(int id, AddDirectorDto dto, CancellationToken token = default)
    {
      var director = await Context.Directors.Where(x => x.Id == id).Project<Director,DirectorDto>(Mapper).SingleOrDefaultAsync(token);

      if (director == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Director), id);
        return NotFound($"No {nameof(Director)} with Id {id} found in database");
      }

      director.FullName = dto.FullName;
      director.Bio = dto.Bio;
      director.BirthDate = dto.BirthDate;

      await Context.SaveChangesAsync(token);
      Logger.LogInformation(LogTemplates.Updated, nameof(Director), director);

      return Ok(director);
    }
  }
}
