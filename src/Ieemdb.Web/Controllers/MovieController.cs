namespace Esentis.Ieemdb.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Joins;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;
  using Esentis.Ieemdb.Web.Models.Enums;
  using Esentis.Ieemdb.Web.Models.SearchCriteria;

  using Kritikos.Extensions.Linq;
  using Kritikos.PureMap;
  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  [Route("api/movie")]
  public class MovieController : BaseController<MovieController>
  {
    public MovieController(ILogger<MovieController> logger, IeemdbDbContext ctx, IPureMapper mapper)
      : base(logger, ctx, mapper)
    {
    }

    /// <summary>
    /// Returns a single Movie.
    /// </summary>
    /// <param name="id">Movie's unique ID. </param>
    /// <response code="404">Movie not found.</response>
    /// <returns>Single <see cref="MovieDto"/>.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovie(long id, CancellationToken token = default)
    {
      var movie = await Context.Movies
        .Include(x => x.People)
        .ThenInclude(x => x.Person)
        .Include(x => x.MovieGenres)
        .ThenInclude(x => x.Genre)
        .Include(x => x.MovieCountries)
        .ThenInclude(x => x.Country)
        .SingleOrDefaultAsync(m => m.Id == id, CancellationToken.None);

      if (movie == null)
      {
        return NotFound("Movie not found");
      }

      return Ok(Mapper.Map<Movie, MovieDto>(movie, "complete"));
    }

    /// <summary>
    /// Searches Movies based on multiple criteria.
    /// </summary>
    /// <param name="criteria">Search criteria.</param>
    /// <response code="200">Returns search results.</response>
    /// <response code="400">Invalid data.</response>
    /// <returns>Search results.</returns>
    [HttpPost("search")]
    public async Task<ActionResult<ICollection<MovieDto>>> SearchForMovie(
      [FromBody] MovieSearchCriteria criteria,
      CancellationToken token = default)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState.Values);
      }

      var query = Context.Movies
        .WhereIf(
          criteria.MinDurationInMinutes != null,
          x => x.Duration >= TimeSpan.FromMinutes(criteria.MinDurationInMinutes!.Value))
        .WhereIf(
          criteria.MaxDurationInMinutes != null,
          x => x.Duration <= TimeSpan.FromMinutes(criteria.MaxDurationInMinutes!.Value))
        .WhereIf(
          criteria.FromYear != null,
          x => x.ReleaseDate >= criteria.FromYear)
        .WhereIf(
          criteria.ToYear != null,
          x => x.ReleaseDate <= criteria.ToYear)
        .WhereIf(
          criteria.Actor != null,
          x => Context.MoviePeople.Where(p => p.Person.KnownFor == DepartmentEnums.Acting)
            .Any(a =>
              a.Person.NormalizedFullName.Contains(criteria.Actor!.NormalizeSearch()) && x.Id == a.Movie.Id))
        .WhereIf(
          criteria.Director != null,
          x => Context.MoviePeople.Where(p => p.Person.KnownFor == DepartmentEnums.Directing)
            .Any(a =>
              a.Person.NormalizedFullName.Contains(criteria.Director!.NormalizeSearch()) && x.Id == a.Movie.Id))
        .WhereIf(
          criteria.Writer != null,
          x => Context.MoviePeople.Where(p => p.Person.KnownFor == DepartmentEnums.Writing)
            .Any(a =>
              a.Person.NormalizedFullName.Contains(criteria.Writer!.NormalizeSearch()) && x.Id == a.Movie.Id))
        .WhereIf(
          criteria.MinRating != null,
          x => Context.Ratings.Any(r => r.Rate >= criteria.MinRating && r.Movie.Id == x.Id))
        .WhereIf(
          criteria.MaxRating != null,
          x => x.Ratings.Average(y => y.Rate) <= criteria.MaxRating)
        .WhereIf(
          criteria.Genres.Length > 0,
          x => x.MovieGenres.Any(gn => criteria.Genres.Contains(gn.Genre.Id)))
        .OrderBy(x => x.Id);

      query = query
        .WhereIf(criteria.IsFeatured != null, x => x.Featured == criteria.IsFeatured)
        .WhereIf(
          criteria.TitleCriteria != null,
          x => x.NormalizedTitle.Contains(criteria.TitleCriteria!.NormalizeSearch()) || EF.Functions
            .ToTsVector("english", x.NormalizedSearch)
            .Matches(criteria.TitleCriteria!))
        .WhereIf(
          criteria.PlotCriteria != null,
          x => EF.Functions.ToTsVector("english", x.NormalizedSearch).Matches(criteria.PlotCriteria!))
        .OrderBy(x => x.Id);

      var matchingMovies = await query.CountAsync(token);

      var slice = await query.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Movie, MovieMinimalDto>(Mapper)
        .ToListAsync(token);

      PagedResult<MovieMinimalDto> results = new()
      {
        Page = criteria.Page,
        Results = slice,
        TotalElements = matchingMovies,
        TotalPages = (matchingMovies / criteria.ItemsPerPage) + 1,
      };

      return Ok(results);
    }

    /// <summary>
    /// Adds a Movie.
    /// </summary>
    /// <param name="dto">Movie information.</param>
    /// <response code="200">Successfully added.</response>
    /// <response code="400">Fields missing.</response>
    /// <response code="404">Missing actors. Missing directors. Missing countries. Missing writers. Missing genres.</response>
    /// <returns>Created <see cref="MovieDto"/>.</returns>
    [HttpPost("")]
    public async Task<ActionResult> AddMovie([FromBody] AddMovieDto dto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState.Values.SelectMany(c => c.Errors));
      }

      var actorIds = dto.ActorIds.Distinct().OrderBy(x => x).ToList();
      var directorIds = dto.DirectorIds.Distinct().OrderBy(x => x).ToList();
      var writerIds = dto.WriterIds.Distinct().OrderBy(x => x).ToList();
      var genreIds = dto.GenreIds.Distinct().OrderBy(x => x).ToList();
      var screenshotUrls = dto.ScreenshotUrls.Distinct().ToList();
      var countryIds = dto.CountryIds.Distinct().OrderBy(x => x).ToList();

      var countries = await Context.Countries.Where(x => countryIds.Contains(x.Id)).ToListAsync();
      var genres = await Context.Genres.Where(x => genreIds.Contains(x.Id)).ToListAsync();
      var actors = await Context.People.Where(p => p.KnownFor == DepartmentEnums.Acting)
        .Where(x => actorIds.Contains(x.Id))
        .ToListAsync();
      var directors = await Context.People.Where(p => p.KnownFor == DepartmentEnums.Directing)
        .Where(x => directorIds.Contains(x.Id))
        .ToListAsync();
      var writers = await Context.People.Where(p => p.KnownFor == DepartmentEnums.Writing)
        .Where(x => writerIds.Contains(x.Id))
        .ToListAsync();

      var missingDirectors = directorIds.Except(directors.Select(a => a.Id)).ToList();
      var missingActors = actorIds.Except(actors.Select(a => a.Id)).ToList();
      var missingGenres = genreIds.Except(genres.Select(a => a.Id)).ToList();
      var missingWriters = writerIds.Except(writers.Select(a => a.Id)).ToList();
      var missingCountries = countryIds.Except(countries.Select(a => a.Id)).ToList();

      if (missingCountries.Count != 0)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Country), missingCountries);
        return NotFound($"Could not find countries with ids {string.Join(", ", missingCountries)}");
      }

      if (missingActors.Count != 0)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Person), missingActors);
        return NotFound($"Could not find actors with ids {string.Join(", ", missingActors)}");
      }

      if (missingDirectors.Count != 0)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Person), missingDirectors);
        return NotFound($"Could not find directors with ids {string.Join(", ", missingDirectors)}");
      }

      if (missingWriters.Count != 0)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Person), missingWriters);
        return NotFound($"Could not find writers with ids {string.Join(", ", missingWriters)}");
      }

      if (missingGenres.Count != 0)
      {
        Logger.LogWarning(LogTemplates.NotFound, nameof(Genre), missingGenres);
        return NotFound($"Could not find genres with ids {string.Join(", ", missingGenres)}");
      }

      var movie = new Movie
      {
        PosterUrl = dto.PosterUrl,
        Featured = false,
        Duration = TimeSpan.FromMinutes(dto.DurationInMinutes),
        Plot = dto.Plot,
        ReleaseDate = dto.ReleaseDate,
        Title = dto.Title,
      };

      var movieActors = actors.Select(x => new MoviePerson { Person = x, Movie = movie }).ToList();
      var movieDirectors = directors.Select(x => new MoviePerson { Person = x, Movie = movie }).ToList();
      var movieWriters = writers.Select(x => new MoviePerson { Person = x, Movie = movie }).ToList();
      var movieGenres = genres.Select(x => new MovieGenre { Genre = x, Movie = movie }).ToList();
      var movieCountries = countries.Select(x => new MovieCountry { Country = x, Movie = movie }).ToList();
      var screenshots = screenshotUrls.Select(x => new Image { Movie = movie, Url = x.ToString() }).ToList();

      Context.MoviePeople.AddRange(movieActors);
      Context.MoviePeople.AddRange(movieDirectors);
      Context.MoviePeople.AddRange(movieWriters);
      Context.MovieGenres.AddRange(movieGenres);
      Context.MovieCountries.AddRange(movieCountries);
      Context.Images.AddRange(screenshots);
      Context.Movies.Add(movie);

      await Context.SaveChangesAsync();

      return Ok();
    }

    /// <summary>
    /// Updates a Movie with new information.
    /// </summary>
    /// <param name="id">Unique ID of the movie to add to featured.</param>
    /// <param name="movieDto">Movie information to be updated.</param>
    /// <response code="200">Movie added to features.</response>
    /// <response code="404">Movie not found.</response>
    /// <returns>Updated <see cref="MovieDto"/>.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<MovieDto>> UpdateMovie(long id, [FromBody] UpdateMovieDto movieDto)
    {
      var movie = await Context.Movies.SingleOrDefaultAsync(x => x.Id == id);
      if (movie == null)
      {
        return NotFound();
      }

      Mapper.Map(movieDto, movie);
      await Context.SaveChangesAsync();

      var dto = Mapper.Map<Movie, MovieMinimalDto>(movie);

      return Ok(dto);
    }

    /// <summary>
    /// Adds a Movie to featured list.
    /// </summary>
    /// <param name="id">Unique ID of the movie to add to featured.</param>
    /// <response code="200">Movie added to features.</response>
    /// <response code="404">Movie not found.</response>
    [HttpPost("feature")]
    public async Task<ActionResult<MovieDto>> AddFeaturedMovie(
      long id,
      CancellationToken cancellationToken = default)
    {
      var movie = await Context.Movies.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

      if (movie == null)
      {
        return NotFound("Movie not found.");
      }

      movie.Featured = true;

      await Context.SaveChangesAsync(cancellationToken);

      return Ok(Mapper.Map<Movie, MovieMinimalDto>(movie));
    }

    /// <summary>
    /// Removes a Movie from featured list.
    /// </summary>
    /// <param name="id">Unique ID of the movie to remove.</param>
    /// <response code="204">Succesfully added to list.</response>
    /// <response code="404">Movie not found.</response>
    /// <returns>No Content.</returns>
    [HttpPost("unfeature")]
    public async Task<ActionResult> RemoveFeaturedMovie(long id, CancellationToken cancellationToken = default)
    {
      var movie = await Context.Movies.SingleOrDefaultAsync(m => m.Id == id, cancellationToken);

      if (movie == null)
      {
        return NotFound("Movie not found.");
      }

      movie.Featured = false;
      await Context.SaveChangesAsync();

      return NoContent();
    }

    /// <summary>
    /// Returns all time top Movies.
    /// </summary>
    /// <response code="200">Succesfully returns movies.</response>
    /// <returns>List of top 100 movies.</returns>
    [HttpGet("top")]
    public async Task<ActionResult<List<MovieDto>>> GetTop(CancellationToken cancellationToken = default)
    {
      var topRatedMovieIds = await Context.Ratings.OrderByDescending(x => x.Rate)
        .Take(100)
        .Select(x => x.Movie.Id)
        .ToListAsync(cancellationToken);

      var topMovies = await Context.Movies.Include(x => x.People)
        .ThenInclude(x => x.Person)
        .Project<Movie, MovieMinimalDto>(Mapper)
        .ToListAsync(cancellationToken);

      return Ok(topMovies);
    }

    /// <summary>
    /// Returns Movies released the current month.
    /// </summary>
    /// <param name="criteria">Page results criteria.</param>
    /// <response code="200">Succesfully returns movies.</response>
    /// <returns>List of movies.</returns>
    [HttpPost("new")]
    public async Task<ActionResult<ICollection<MovieDto>>> GetNewReleases(
      PaginationCriteria criteria,
      CancellationToken token = default)
    {
      var monthAgo = DateTimeOffset.Now.AddDays(-30);
      var now = DateTimeOffset.Now;

      var newMoviesQuery = Context.Movies.Include(x => x.People)
        .ThenInclude(x => x.Person)
        .Where(x => (x.ReleaseDate <= now) && (x.ReleaseDate >= monthAgo))
        .OrderBy(x => x.ReleaseDate);

      var moviesCount = await newMoviesQuery.CountAsync(token);

      var slice = await newMoviesQuery.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Movie, MovieMinimalDto>(Mapper)
        .ToListAsync(token);

      PagedResult<MovieMinimalDto> results = new()
      {
        Page = criteria.Page,
        Results = slice,
        TotalElements = moviesCount,
        TotalPages = (moviesCount / criteria.ItemsPerPage) + 1,
      };
      return Ok(results);
    }

    /// <summary>
    /// Returns Movies released the current week.
    /// </summary>
    /// <param name="criteria">Page results criteria.</param>
    /// <response code="200">Succesfully returns movies.</response>
    /// <returns>List of movies.</returns>
    [HttpPost("latest")]
    public async Task<ActionResult<ICollection<MovieDto>>> GetLatestAdded(
      PaginationCriteria criteria,
      CancellationToken token = default)
    {
      var weekAgo = DateTimeOffset.Now.AddDays(-7);
      var now = DateTimeOffset.Now;

      var latestMoviesQuery = Context.Movies.Include(x => x.People)
        .ThenInclude(x => x.Person)
        .Where(x => (x.CreatedAt <= now) && (x.CreatedAt >= weekAgo))
        .OrderBy(x => x.CreatedAt);

      var moviesCount = await latestMoviesQuery.CountAsync(token);

      var slice = await latestMoviesQuery.Slice(criteria.Page, criteria.ItemsPerPage)
        .Project<Movie, MovieMinimalDto>(Mapper)
        .ToListAsync(token);

      PagedResult<MovieMinimalDto> results = new()
      {
        Page = criteria.Page,
        Results = slice,
        TotalElements = moviesCount,
        TotalPages = (moviesCount / criteria.ItemsPerPage) + 1,
      };

      return Ok(results);
    }

    /// <summary>
    /// Removes a Movie.
    /// </summary>
    /// <param name="id">Movie's unique ID.</param>
    /// <response code="204">Succesfully deleted.</response>
    /// <response code="404">Movie not found.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMovie(long id, CancellationToken token = default)
    {
      var movie = await Context.Movies.SingleOrDefaultAsync(x => x.Id == id, token);

      if (movie == null)
      {
        return NotFound("Movie not found");
      }

      movie.IsDeleted = true;

      await Context.SaveChangesAsync(token);
      return NoContent();
    }

    /// <summary>
    /// Returns the images associated with the movie.
    /// </summary>
    /// <param name="id">Movie's unique ID.</param>
    /// <response code="204">Succesfully deleted.</response>
    /// <response code="404">Images not found.</response>
    /// <returns>Returns a list of <see cref="ImageDto"/></returns>
    [HttpGet("{id}/images")]
    public async Task<ActionResult<List<ImageDto>>> GetImages(long id, CancellationToken token = default)
    {
      var images = await Context.Images.Include(i => i.Movie)
        .Where(i => i.Movie.Id == id)
        .Project<Image, ImageDto>(Mapper)
        .ToListAsync(token);

      if (images == null)
      {
        return NotFound("Images not found");
      }

      await Context.SaveChangesAsync(token);
      return Ok(images);
    }

    /// <summary>
    /// Returns the videos associated with the movie.
    /// </summary>
    /// <param name="id">Movie's unique ID.</param>
    /// <response code="204">Returns a list of videos.</response>
    /// <response code="404">Videos not found.</response>
    /// <returns>Returns a list of <see cref="VideoDto"/></returns>
    [HttpGet("{id}/videos")]
    public async Task<ActionResult<List<VideoDto>>> GetVideos(long id, CancellationToken token = default)
    {
      var videos = await Context.Videos.Include(i => i.Movie)
        .Where(i => i.Movie.Id == id)
        .Project<Video, VideoDto>(Mapper)
        .ToListAsync(token);

      if (videos == null)
      {
        return NotFound("Movie not found");
      }

      await Context.SaveChangesAsync(token);
      return Ok(videos);
    }
  }
}
