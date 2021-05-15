namespace Esentis.Ieemdb.Web.Controllers
{
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models.Dto;

  using Kritikos.PureMap;
  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Authorize]
  [Route("api/country")]
  public class CountryController : BaseController<CountryController>
  {
    public CountryController(ILogger<CountryController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    /// <summary>
    /// Returns all Countries.
    /// </summary>
    /// <returns>List of <see cref="CountryDto"/>.</returns>
    [AllowAnonymous]
    [HttpGet("")]
    public async Task<ActionResult<List<CountryDto>>> GetCountries(CancellationToken token = default)
    {
      var countries = await Context.Countries.Project<Country, CountryDto>(Mapper).ToListAsync(token);
      return Ok(countries);
    }

    /// <summary>
    /// Adds a Country.
    /// </summary>
    /// <param name="name">Country's name.</param>
    /// <response code="200">Returns newly created country.</response>
    /// <response code="400">Name should not be empty.</response>
    /// <returns>Created <see cref="Country"/>.</returns>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost("")]
    public async Task<ActionResult> AddCountry([FromBody] string name, CancellationToken token = default)
    {
      if (name.Length == 0)
      {
        return BadRequest("Name should not be empty !");
      }

      var country = new Country { Name = name };
      Context.Countries.Add(country);
      await Context.SaveChangesAsync(token);
      return Ok(country);
    }

    /// <summary>
    /// Removes a Country.
    /// </summary>
    /// <param name="id">Country's unique ID.</param>
    /// <response code="204">Successfully deleted.</response>
    /// <response code="404">Country not found.</response>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveCountry(long id, CancellationToken token = default)
    {
      var country = await Context.Countries.SingleOrDefaultAsync(c => c.Id == id, token);

      if (country == null || country.IsDeleted)
      {
        return NotFound("Country not found");
      }

      country.IsDeleted = true;

      await Context.SaveChangesAsync(token);
      return NoContent();
    }
  }
}
