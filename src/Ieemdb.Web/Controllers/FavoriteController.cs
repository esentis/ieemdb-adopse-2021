namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;

  using Kritikos.PureMap;
  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Authorize]
  [Route("api/favorite")]
  public class FavoriteController : BaseController<MovieController>
  {
    private readonly UserManager<IeemdbUser> userManager;

    public FavoriteController(ILogger<MovieController> logger, IeemdbDbContext ctx, IPureMapper mapper,
      UserManager<IeemdbUser> userManager)
      : base(logger, ctx, mapper)
    {
      this.userManager = userManager;
    }

    /// <summary>
    /// Adds a movie to favorites.
    /// </summary>
    /// <param name="movieId">Movie's unique ID.</param>
    /// <response code="201">Movie successfuly rated.</response>
    /// <response code="400">User error.</response>
    /// <response code="404">Movie not found.</response>
    /// <response code="409">User has already favorited the movie.</response>
    /// <returns>No content.</returns>
    [HttpPost("")]
    public async Task<ActionResult> AddFavorite(long movieId, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return BadRequest("Something went wrong.");
      }

      var movie = await Context.Movies.SingleOrDefaultAsync(m => m.Id == movieId, token);

      if (movie == null)
      {
        return NotFound("Movie not found");
      }

      var isFavorited = await Context.Favorites.AnyAsync(f => f.Movie == movie && f.User == user, token);

      if (isFavorited)
      {
        return Conflict("User has already favorited the movie");
      }

      var favoriteMovie = new Favorite { Movie = movie, User = user };

      Context.Favorites.Add(favoriteMovie);

      await Context.SaveChangesAsync();
      return NoContent();
    }

    /// <summary>
    /// Returns user's favorited movies.
    /// </summary>
    /// <response code="200">Returns the list of favorited movies.</response>
    /// <response code="400">User error.</response>
    /// <returns>List of <see cref="MovieDto"/>.</returns>
    [HttpGet("")]
    public async Task<ActionResult<ICollection<MovieDto>>> GetFavorites(CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return BadRequest("Something went wrong.");
      }

      var favoritedMoviesIds = await Context.Favorites.Where(f => f.User == user)
        .Select(x => x.Movie)
        .Select(x => x.Id)
        .ToListAsync(token);

      var favoriteMovies = await Context.Movies.Include(x => x.MovieActors)
        .ThenInclude(x => x.Actor)
        .Include(x => x.MovieDirectors)
        .ThenInclude(x => x.Director)
        .Include(x => x.MovieWriters)
        .ThenInclude(x => x.Writer)
        .Include(x => x.MovieGenres)
        .ThenInclude(x => x.Genre)
        .Include(x => x.MovieCountries)
        .ThenInclude(x => x.Country)
        .Where(mv => favoritedMoviesIds.Contains(mv.Id))
        .Project<Movie, MovieDto>(Mapper, "complete")
        .ToListAsync(token);

      return Ok(favoriteMovies);
    }

    /// <summary>
    /// Removes a movie from favorites.
    /// </summary>
    /// <param name="movieId">Movie's unique ID.</param>
    /// <response code="204">No content returned.</response>
    /// <response code="400">User error.</response>
    /// <response code="404">Favorite not found.</response>
    /// <returns>No content.</returns>
    [HttpDelete("")]
    public async Task<ActionResult<ICollection<MovieDto>>> RemoveFavorite(
      long movieId,
      CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return BadRequest("Something went wrong.");
      }

      var movie = await Context.Favorites.SingleOrDefaultAsync(f => f.User == user && f.Movie.Id == movieId, token);

      if (movie == null)
      {
        return NotFound("Favorite not found");
      }

      Context.Favorites.Remove(movie);
      await Context.SaveChangesAsync();

      return NoContent();
    }
  }
}
