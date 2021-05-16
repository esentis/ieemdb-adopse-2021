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

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Authorize]
  [Route("api/genre")]
  [ApiController]
  public class GenreController : BaseController<GenreController>
  {
    public GenreController(ILogger<GenreController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    /// <summary>
    /// Returns all Genres.
    /// </summary>
    /// <param name="criteria">Pagination criteria.</param>
    /// <response code="200">Succesfully returns Genres.</response>
    /// <response code="400">Page doesn't exist.</response>
    /// <returns>List of <see cref="GenreDto"/>.</returns>
    [AllowAnonymous]
    [HttpPost("all")]
    public async Task<ActionResult<List<GenreDto>>> GetGenres(
      PaginationCriteria criteria,
      CancellationToken token = default)
    {
      var genresQuery = Context.Genres
        .TagWith("Retrieving all genres")
        .OrderBy(x => x.Id);

      var totalGenres = await genresQuery.CountAsync(token);

      if (criteria.Page > ((totalGenres / criteria.ItemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist");
      }

      var pagedGenres = await genresQuery.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Genre, GenreDto>(Mapper)
        .ToListAsync(token);

      var result = new PagedResult<GenreDto>
      {
        Results = pagedGenres,
        Page = criteria.Page,
        TotalPages = (totalGenres / criteria.ItemsPerPage) + 1,
        TotalElements = totalGenres,
      };

      return Ok(result);
    }

    /// <summary>
    /// Returns a single Genre.
    /// </summary>
    /// <param name="id">Genre's unique ID.</param>
    /// <response code="200">Succesfully gets a Genre.</response>
    /// <response code="404">Genre not found.</response>
    /// <returns>Single <see cref="GenreDto"/>.</returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDto>> GetGenre(long id, CancellationToken token = default)
    {
      var genre = await Context.Genres.Where(x => x.Id == id).SingleOrDefaultAsync(token);

#pragma warning disable IDE0046 // Waiting for the new C# version
      if (genre == null)
#pragma warning restore IDE0046 // Convert to conditional expression
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Genre), id);
        return NotFound($"No {nameof(Genre)} with Id {id} found in database");
      }

      Logger.LogInformation(LogTemplates.RequestEntity, nameof(Genre), id);

      return Ok(Mapper.Map<Genre, GenreDto>(genre));
    }

    /// <summary>
    /// Adds a Genre.
    /// </summary>
    /// <param name="dto">Genre information.</param>
    /// <response code="201">Succesfully created.</response>
    /// <response code="401">Unauthorized.</response>
    /// <returns>Created <see cref="GenreDto"/>.</returns>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost("")]
    public async Task<ActionResult<GenreDto>> AddGenre(
      [FromBody] AddGenreDto dto,
      CancellationToken token = default)
    {
      var genre = Mapper.Map<AddGenreDto, Genre>(dto);

      Context.Genres.Add(genre);

      await Context.SaveChangesAsync(token);
      Logger.LogInformation(LogTemplates.CreatedEntity, nameof(Genre), genre);

      return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, Mapper.Map<Genre, GenreDto>(genre));
    }

    /// <summary>
    /// Deletes a Genre.
    /// </summary>
    /// <response code="204">Successfully deleted.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Genre not found.</response>
    /// <param name="id">Genre's unique ID.</param>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpDelete("")]
    public async Task<ActionResult> DeleteGenre(int id, CancellationToken token = default)
    {
      var genre =await Context.Genres.Where(x => x.Id == id).SingleOrDefaultAsync(token);

      if (genre == null || genre.IsDeleted)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Genre), id);
        return NotFound("No genre found in the database");
      }

      genre.IsDeleted = true;

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Deleted, nameof(Genre), id);

      // An empty success response.
      return NoContent();
    }

    /// <summary>
    /// Updates a Genre.
    /// </summary>
    /// <param name="id">Genre's unique ID.</param>
    /// <param name="dto">Genre's information to be updated.</param>
    /// <response code="200">Succesfully updated.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Genre not found.</response>
    /// <returns>Updated <see cref="GenreDto"/>.</returns>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPut("{id}")]
    public async Task<ActionResult<GenreDto>> UpdateGenre(int id, AddGenreDto dto, CancellationToken token = default)
    {
      var genre = await Context.Genres.Where(x => x.Id == id).SingleOrDefaultAsync(token);

      if (genre == null)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Genre), id);
        return NotFound($"No {nameof(Genre)} with Id {id} found in database");
      }

      genre.Name = dto.Name;

      await Context.SaveChangesAsync();
      Logger.LogInformation(LogTemplates.Updated, nameof(Genre), genre);

      return Ok(Mapper.Map<Genre, GenreDto>(genre));
    }
  }
}
