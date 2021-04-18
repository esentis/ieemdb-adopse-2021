namespace Esentis.Ieemdb.Web.Helpers
{
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;

  using Kritikos.PureMap;
  using Kritikos.PureMap.Contracts;

  using Nessos.Expressions.Splicer;

  public static class MappingConfiguration
  {
    public static readonly IPureMapperConfig Mapping = new PureMapperConfig()
      .Map<Actor, ActorDto>(mapper => actor => new ActorDto() { Name = actor.Name, Bio = actor.Bio, Id = actor.Id, BirthDate = actor.BirthDate, })
      .Map<ActorDto, Actor>(mapper => actorDto => new Actor() { Name = actorDto.Name, Bio = actorDto.Bio, Id = actorDto.Id, BirthDate = actorDto.BirthDate, })
      .Map<AddActorDto, Actor>(mapper => addActor => new Actor() { Name = addActor.Name, Bio = addActor.Bio, BirthDate = addActor.BirthDate, })
      .Map<Director, DirectorDto>(mapper => director => new DirectorDto() { Name = director.Name, Bio = director.Bio, Id = director.Id, BirthDate = director.BirthDate, })
      .Map<DirectorDto, Director>(mapper => directorDto => new Director() { Name = directorDto.Name, Bio = directorDto.Bio, Id = directorDto.Id, BirthDate = directorDto.BirthDate, })
      .Map<AddDirectorDto, Director>(mapper => addDirector => new Director() { Name = addDirector.Name, Bio = addDirector.Bio, BirthDate = addDirector.BirthDate, })
      .Map<Genre, GenreDto>(mapper => genre => new GenreDto() { Name = genre.Name, Id = genre.Id, })
      .Map<GenreDto, Genre>(mapper => genreDto => new Genre() { Name = genreDto.Name, Id = genreDto.Id, })
      .Map<AddGenreDto, Genre>(mapper => addGenre => new Genre() { Name = addGenre.Name, })
      .Map<Writer, WriterDto>(mapper => writer => new WriterDto() { Name = writer.Name, Bio = writer.Bio, Id = writer.Id, BirthDate = writer.BirthDate, })
      .Map<WriterDto, Writer>(mapper => writerDto => new Writer() { Name = writerDto.Name, Bio = writerDto.Bio, Id = writerDto.Id, BirthDate = writerDto.BirthDate, })
      .Map<AddWriterDto, Writer>(mapper => addWriter => new Writer() { Name = addWriter.Name, Bio = addWriter.Bio, BirthDate = addWriter.BirthDate, })
      .Map<MovieDto, Movie>(mapper => (dest, source) => UpdateMovie(source, dest))
      .Map<MovieDto, Movie>(mapper => (dest, source) => UpdateMovie(source, dest), name: "v2");

    private static Movie UpdateMovie(Movie movie, MovieDto dto)
    {
      return movie;
    }
  }
}
