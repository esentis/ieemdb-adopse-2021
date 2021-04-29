namespace Esentis.Ieemdb.Web
{
  using System;

  using Bogus;

  using Esentis.Ieemdb.Web.Models;
  using Esentis.Ieemdb.Web.Models.Dto;

  public static class Fakers
  {
    private static int order = 0;

    public static readonly Faker<ActorDto> ActorProvider =
      new Faker<ActorDto>()
        .RuleFor(e => e.FirstName, f => f.Person.FirstName)
        .RuleFor(e => e.LastName, f => f.Person.LastName)
        .RuleFor(e => e.BirthDate, f => f.Date.Past())
        .RuleFor(e => e.Bio, f => f.Lorem.Sentence(5, 20))
        .RuleFor(e => e.Id, f => f.IndexFaker);

    public static readonly Faker<DirectorDto> DirectorProvider =
      new Faker<DirectorDto>()
        .RuleFor(e => e.FirstName, f => f.Person.FirstName)
        .RuleFor(e => e.LastName, f => f.Person.LastName)
        .RuleFor(e => e.BirthDate, f => f.Date.Past())
        .RuleFor(e => e.Bio, f => f.Lorem.Sentence(5, 20))
        .RuleFor(e => e.Id, f => f.IndexFaker);

    public static readonly Faker<WriterDto> WriterProvider =
      new Faker<WriterDto>()
        .RuleFor(e => e.FirstName, f => f.Person.FirstName)
        .RuleFor(e => e.LastName, f => f.Person.LastName)
        .RuleFor(e => e.BirthDate, f => f.Date.Past())
        .RuleFor(e => e.Bio, f => f.Lorem.Sentence(5, 20))
        .RuleFor(e => e.Id, f => f.IndexFaker);

    public static readonly Faker<CountryDto> CountryProvider =
      new Faker<CountryDto>()
        .RuleFor(e => e.Name, f => f.Person.FirstName);

    public static readonly Faker<GenreDto> GenreProvider =
      new Faker<GenreDto>()
        .RuleFor(e => e.Name, f => f.Random.Word());

    public static readonly Faker<ImageDto> ImageProvider =
      new Faker<ImageDto>()
        .RuleFor(e => e.Url, f => f.Image.PicsumUrl(width: 792, height: 1008).ToString());

    public static readonly Faker<ImageDto> BannerProvider =
      new Faker<ImageDto>()
        .RuleFor(e => e.Url, f => f.Image.PicsumUrl(width: 970, height: 250).ToString());

    public static readonly Faker<MovieDto> MovieProvider =
      new Faker<MovieDto>()
        .RuleFor(e => e.Id, f => order++)
        .RuleFor(e => e.Title, f => f.Random.Words(3))
        .RuleFor(e => e.Plot, f => f.Rant.Random.Words(14))
        .RuleFor(e => e.TrailerUrl, f => f.Internet.Url())
        .RuleFor(e => e.Duration, f => f.Date.Timespan(maxSpan: TimeSpan.FromHours(2)))
        .RuleFor(e => e.Featured, f => f.Random.Bool())
        .RuleFor(e => e.Rating, f => f.Random.Double(0, 10))
        .RuleFor(e => e.Genres, f => GenreProvider.Generate(3))
        .RuleFor(e => e.ReleaseDate, f => f.Date.Past(5))
        .RuleFor(e => e.Actors, f => ActorProvider.Generate(5))
        .RuleFor(e => e.Directors, f => DirectorProvider.Generate(2))
        .RuleFor(e => e.Writers, f => WriterProvider.Generate(3));
  }
}
