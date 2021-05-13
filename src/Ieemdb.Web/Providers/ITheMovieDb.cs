namespace Esentis.Ieemdb.Web.Providers
{
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Web.Models.MovieDBDto;
  using Esentis.Ieemdb.Web.Providers.Models;

  using Refit;

  [Headers("Authorization: Bearer")]
  public interface ITheMovieDb
  {
    [Get("/search/person")]
    Task<SearchResult<PersonSearch>> SearchPeople(string query, int page = 1);

    [Get("/person/{id}")]
    Task<PersonDB> GetPerson(long id);

    [Get("/genre/movie/list")]
    Task<GenreResults> GetGenres();

    [Get("/configuration/countries")]
    Task<CountryDB[]> GetCountries();

    [Get("/movie/popular")]
    Task<SearchResult<MovieDB>> GetPopular(int page = 1);

    [Get("/movie/{id}")]
    Task<DetailedMovie> GetMovieDetails(long id);

    [Get("/movie/{id}/credits")]
    Task<MovieCast> GetMovieCredits(long id);

    [Get("/movie/{id}/videos")]
    Task<VideoResults> GetMovieVideos(long id);

    [Get("/movie/{id}/images")]
    Task<ImageResults> GetMovieImages(long id);
  }

  public class FakeMovieDb : ITheMovieDb
  {
    #region Implementation of ITheMovieDb

    /// <inheritdoc />
    public async Task<SearchResult<PersonSearch>> SearchPeople(string query, int page = 1) =>
      new SearchResult<PersonSearch>();

    /// <inheritdoc />
    public async Task<PersonDB> GetPerson(long id) => new PersonDB();

    /// <inheritdoc />
    public async Task<GenreResults> GetGenres() => new GenreResults();

    /// <inheritdoc />
    public async Task<SearchResult<MovieDB>> GetPopular(int page = 1) => new SearchResult<MovieDB>();

    /// <inheritdoc />
    public async Task<DetailedMovie> GetMovieDetails(long id) => new DetailedMovie();

    /// <inheritdoc />
    public async Task<MovieCast> GetMovieCredits(long id) => new MovieCast();

    /// <inheritdoc />
    public async Task<VideoResults> GetMovieVideos(long id) => new VideoResults();

    /// <inheritdoc />
    public async Task<ImageResults> GetMovieImages(long id) => new ImageResults();

    public async Task<CountryDB[]> GetCountries() => new CountryDB[1];

    #endregion
  }
}
