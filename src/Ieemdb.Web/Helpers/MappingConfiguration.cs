namespace Esentis.Ieemdb.Web.Helpers
{
  using System.Linq;
  using System.Threading.Tasks;

  using AutoMapper;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;

  using Kritikos.PureMap;
  using Kritikos.PureMap.Contracts;

  using Microsoft.EntityFrameworkCore;

  using Nessos.Expressions.Splicer;

  public static class MappingConfiguration
  {

    public static readonly IPureMapperConfig Mapping = new PureMapperConfig()
      .Map<Actor, ActorDto>(mapper => actor => new ActorDto() { FirstName = actor.FirstName, LastName = actor.LastName, Bio = actor.Bio, Id = actor.Id, BirthDate = actor.BirthDate, })
      .Map<ActorDto, Actor>(mapper => actorDto => new Actor() { FirstName = actorDto.FirstName, LastName = actorDto.LastName, Bio = actorDto.Bio, Id = actorDto.Id, BirthDate = actorDto.BirthDate, })
      .Map<AddActorDto, Actor>(mapper => addActor => new Actor() { FirstName = addActor.FirstName, LastName = addActor.LastName, Bio = addActor.Bio, BirthDate = addActor.BirthDate, })
      .Map<Director, DirectorDto>(mapper => director => new DirectorDto() { FirstName = director.FirstName, LastName = director.LastName, Bio = director.Bio, Id = director.Id, BirthDate = director.BirthDate, })
      .Map<DirectorDto, Director>(mapper => directorDto => new Director() { FirstName = directorDto.FirstName, LastName = directorDto.LastName, Bio = directorDto.Bio, Id = directorDto.Id, BirthDate = directorDto.BirthDate, })
      .Map<AddDirectorDto, Director>(mapper => addDirector => new Director() { FirstName = addDirector.FirstName, LastName = addDirector.LastName, Bio = addDirector.Bio, BirthDate = addDirector.BirthDate, })
      .Map<Genre, GenreDto>(mapper => genre => new GenreDto() { Name = genre.Name, Id = genre.Id, })
      .Map<Poster, ImageDto>(mapper => poster => new ImageDto() { Url = poster.Url.ToString() })
      .Map<Country, CountryDto>(mapper => country => new CountryDto() { Name = country.Name, Id = country.Id})
      .Map<Screenshot, ImageDto>(mapper => screenshot => new ImageDto() { Url = screenshot.Url.ToString() })
      .Map<GenreDto, Genre>(mapper => genreDto => new Genre() { Name = genreDto.Name, Id = genreDto.Id, })
      .Map<AddGenreDto, Genre>(mapper => addGenre => new Genre() { Name = addGenre.Name, })
      .Map<Writer, WriterDto>(mapper => writer => new WriterDto() { FirstName = writer.FirstName, LastName = writer.LastName, Bio = writer.Bio, Id = writer.Id, BirthDate = writer.BirthDate, })
      .Map<WriterDto, Writer>(mapper => writerDto => new Writer() { FirstName = writerDto.FirstName, LastName = writerDto.LastName, Bio = writerDto.Bio, Id = writerDto.Id, BirthDate = writerDto.BirthDate, })
      .Map<AddWriterDto, Writer>(mapper => addWriter => new Writer() { FirstName = addWriter.FirstName, LastName = addWriter.LastName, Bio = addWriter.Bio, BirthDate = addWriter.BirthDate, })
      .Map<Movie, MovieDto>(mapper => movie => new MovieDto() { Id = movie.Id, Title = movie.Title, Plot = movie.Plot, TrailerUrl = movie.TrailerUrl, Duration = movie.Duration, Featured = movie.Featured, ReleaseDate = movie.ReleaseDate })
      .Map<MovieDto, Movie>(mapper => (dest, source) => UpdateMovie(source, dest))
      .Map<MovieDto, Movie>(mapper => (dest, source) => UpdateMovie(source, dest), name: "v2");

    private static Movie UpdateMovie(Movie movie, MovieDto dto)
    {
      movie.Duration = dto.Duration;
      return movie;
    }

  }
}
