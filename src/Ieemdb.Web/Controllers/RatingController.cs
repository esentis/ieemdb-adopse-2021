namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Net;
  using System.Security.Claims;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;

  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Route("api/rating")]
  public class RatingController : BaseController<RatingController>
  {

    private readonly UserManager<IeemdbUser> userManager;

    public RatingController(ILogger<RatingController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    /// <summary>
    /// Add a new rating to a specific movie.
    /// </summary>
    /// <param name="addRatingDto">Provide movie ID, rate and text Review. </param>
    /// <response code="200">Movie successfuly rated. </response>
    /// <response code="400">Something went wrong. </response>
    /// <response code="402">User has already rated the movie. </response>
    /// <response code="404">Movie not found. </response>
    /// <returns>No Content.</returns>
    [HttpPost("add")]
    public async Task<ActionResult> AddRating(AddRatingDto addRatingDto, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return BadRequest("Something went wrong. ");
      }

      var movie = await Context.Movies.FirstOrDefaultAsync(x => x.Id == addRatingDto.MovieId, token);
      if (movie == null)
      {
        return NotFound("Movie not found. ");
      }

      var rating = await Context.Ratings
        .SingleOrDefaultAsync(x => x.User == user && x.Movie == movie, token);

      // If user is trying to add a rating that he has already added
      if (rating != null && rating.Rate == addRatingDto.Rate)
      {
        return StatusCode(402, "User has already rated the movie. ");
      }

      rating = new Rating { Movie = movie, User = user, Rate = addRatingDto.Rate };
      Context.Ratings.Add(rating);

      await Context.SaveChangesAsync(token);
      return Ok("Movie successfuly rated. ");
    }

    /// <summary>
    /// Remove a rating from a specific movie.
    /// </summary>
    /// <param name="movieId">Movie's unique ID. </param>
    /// <response code="200">Movie successfuly rated. </response>
    /// <response code="400">Something went wrong. </response>
    /// <response code="401">User has already rated the movie. </response>
    /// <response code="402">No rating found. </response>
    /// <response code="404">Movie not found. </response>
    /// <returns>No Content.</returns>
    [HttpDelete("delete")]
    public async Task<ActionResult> RemoveRating(long movieId, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return BadRequest("Something went wrong. ");
      }

      var movie = await Context.Movies.SingleOrDefaultAsync(x => x.Id == movieId);
      if (movie == null)
      {
        Logger.LogInformation(LogTemplates.NotFound, nameof(Movie));
        return NotFound("Movie not found");
      }

      var rating =
        await Context.Ratings.SingleOrDefaultAsync(x => x.Movie.Id == movieId && x.User.Id == user.Id);
      if (rating == null)
      {
        return StatusCode(402, "No rating found. ");
      }

      Context.Ratings.Remove(rating);
      await Context.SaveChangesAsync();
      return Ok("Rating removed");
    }

    /// <summary>
    /// Get all personal ratings of the requester.
    /// </summary>
    /// <param name="itemsPerPage">Defines how many items should be returned per page. </param>
    /// <param name="page">Defines the results' page. </param>
    /// <response code="200">Movie successfuly rated. </response>
    /// <response code="400">Something went wrong. </response>
    /// <response code="402">Page doesn't exist. </response>
    /// <returns>Returns a list of Ratings.</returns>
    [HttpGet("")]
    public async Task<ActionResult<List<Rating>>> PersonalRatings([Range(1, 100)] int itemsPerPage = 20, int page = 1)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return BadRequest("Something went wrong. ");
      }

      var toSkip = itemsPerPage * (page - 1);
      var ratings = Context.Ratings.Include(x => x.Movie)
        .Where(x => x.User.Id == user.Id)
        .OrderBy(x => x.CreatedAt);
      var totalRatings = await ratings.CountAsync();
      var pagedRatings = await ratings
        .Skip(toSkip)
        .Take(itemsPerPage)
        .ToListAsync();
      var result = new PagedResult<RatingDto>
      {
        Results = pagedRatings.Select(x => Mapper.Map<Rating, RatingDto>(x)).ToList(),
        Page = page,
        TotalPages = (totalRatings / itemsPerPage) + 1,
        TotalElements = totalRatings,
      };

      if (page > ((totalRatings / itemsPerPage) + 1))
      {
        return StatusCode(402, "Page doesn't exist. ");
      }

      return Ok(result);

    }

    /// <summary>
    /// Get rating for a specific movie for the current user.
    /// </summary>
    /// <param name="movieId">Movie's unique ID. </param>
    /// <response code="200">Movie successfuly rated. </response>
    /// <response code="400">Something went wrong. </response>
    /// <response code="404">No rating found from the user. </response>
    /// <returns>Returns the Rating for the movie. </returns>
    [HttpPost("check")]
    public async Task<ActionResult<RatingDto>> GetRating(long movieId, CancellationToken token = default)
    {
      var userId = RetrieveUserId().ToString();
      var user = await userManager.FindByIdAsync(userId);

      if (user == null)
      {
        return BadRequest("Something went wrong. ");
      }

      var rating = await Context.Ratings.Where(x => x.Movie.Id == movieId && x.User.Id == user.Id).SingleOrDefaultAsync(cancellationToken: token);
      if (rating == null)
      {
        return NotFound($"No rating found for user {user.Id} for the specific movie. ");
      }

      return Ok(Mapper.Map<Rating, RatingDto>(rating));

    }
  }
}