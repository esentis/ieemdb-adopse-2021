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
  using Esentis.Ieemdb.Web.Models.SearchCriteria;

  using Kritikos.Extensions.Linq;
  using Kritikos.PureMap;
  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Authorize]
  [Route("api/rating")]
  public class RatingController : BaseController<RatingController>
  {
    private readonly UserManager<IeemdbUser> userManager;

    public RatingController(ILogger<RatingController> logger, IeemdbDbContext ctx, IPureMapper mapper,
      UserManager<IeemdbUser> userManager)
      : base(logger, ctx, mapper)
    {
      this.userManager = userManager;
    }

    /// <summary>
    /// Add a new rating.
    /// </summary>
    /// <param name="addRatingDto">Provide movie ID, rate and text Review.</param>
    /// <response code="201">Movie successfuly rated.</response>
    /// <response code="400">No such user.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Movie not found.</response>
    /// <response code="409">User has already rated the movie.</response>
    /// <returns>Created <see cref="RatingDto"/>.</returns>
    [HttpPost("")]
    public async Task<ActionResult<RatingDto>> AddRating(AddRatingDto addRatingDto, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();

      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return BadRequest("No such user.");
      }

      var movie = await Context.Movies.Include(x => x.Ratings)
        .FirstOrDefaultAsync(x => x.Id == addRatingDto.MovieId, token);
      if (movie == null)
      {
        return NotFound("Movie not found.");
      }

      var rating = await Context.Ratings
        .SingleOrDefaultAsync(x => x.User == user && x.Movie == movie, token);

      // If user is trying to add a rating that he has already added
      if (rating != null)
      {
        return Conflict("User has already rated the movie.");
      }

      movie.AverageRating = (movie.Ratings.Sum(x => x.Rate) + addRatingDto.Rate) / (movie.Ratings.Count + 1);
      rating = new Rating { Movie = movie, User = user, Rate = addRatingDto.Rate, Review = addRatingDto.Review };

      Context.Ratings.Add(rating);

      await Context.SaveChangesAsync(token);

      return CreatedAtAction(nameof(GetRating), new { id = rating.Id }, Mapper.Map<Rating, RatingDto>(rating));
    }

    /// <summary>
    /// Remove a rating.
    /// </summary>
    /// <param name="movieId">Movie's unique ID.</param>
    /// <response code="204">Movie successfuly rated.</response>
    /// <response code="400">Something went wrong.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">No rating found.</response>
    /// <returns>No Content.</returns>
    [HttpDelete("delete")]
    public async Task<ActionResult> RemoveRating(long movieId, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return BadRequest("Something went wrong.");
      }

      var rating =
        await Context.Ratings
          .Include(x => x.Movie.Ratings)
          .SingleOrDefaultAsync(x => x.Movie.Id == movieId && x.User.Id == user.Id, token);
      if (rating == null)
      {
        return NotFound("No rating found.");
      }

      double result = rating.Movie.Ratings.Where(x => x.Id != rating.Id).Sum(x => x.Rate) / (rating.Movie.Ratings.Count - 1);

      if (!double.IsFinite(result))
      {
        rating.Movie.AverageRating = 0;
      }
      else
      {
        rating.Movie.AverageRating = result;
      }

      Context.Ratings.Remove(rating);
      await Context.SaveChangesAsync(token);
      return NoContent();
    }

    /// <summary>
    /// Get all personal ratings of the requester.
    /// </summary>
    /// <param name="criteria"><see cref="PaginationCriteria"/>.</param>
    /// <response code="200">Movie successfuly rated.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="400">User error.</response>
    /// <returns>Returns a list of Ratings.</returns>
    [HttpPost("personal")]
    public async Task<ActionResult<List<Rating>>> PersonalRatings(PaginationCriteria criteria, CancellationToken token = default)
    {
      var userId = Guid.Parse(RetrieveUserId().ToString());

      var ratings = Context.Ratings.Include(x => x.Movie)
        .Where(x => x.User.Id == userId)
        .OrderBy(x => x.CreatedAt);
      var totalRatings = await ratings.CountAsync(token);
      var pagedRatings = await ratings.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Rating, RatingDto>(Mapper)
        .ToListAsync(token);
      var result = new PagedResult<RatingDto>
      {
        Results = pagedRatings,
        Page = criteria.Page,
        TotalPages = (totalRatings / criteria.ItemsPerPage) + 1,
        TotalElements = totalRatings,
      };

      if (criteria.Page > ((totalRatings / criteria.ItemsPerPage) + 1))
      {
        return BadRequest("Page doesn't exist.");
      }

      return Ok(result);
    }

    /// <summary>
    /// Get rating for a specific movie for the current user.
    /// </summary>
    /// <param name="movieId">Movie's unique ID. </param>
    /// <response code="200">Movie successfuly rated. </response>
    /// <response code="400">Something went wrong. </response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">No rating found from the user. </response>
    /// <returns>Returns the Rating for the movie. </returns>
    [HttpPost("check")]
    public async Task<ActionResult<RatingDto>> GetRating(long movieId, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);

      if (user == null)
      {
        return BadRequest("Something went wrong.");
      }

      var rating = await Context.Ratings.Where(x => x.Movie.Id == movieId && x.User.Id == user.Id)
        .Project<Rating, RatingDto>(Mapper)
        .SingleOrDefaultAsync(token);
      if (rating == null)
      {
        return NotFound($"No rating found for user {user.Id} for the specific movie.");
      }

      return Ok(rating);
    }
  }
}
