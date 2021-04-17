namespace Esentis.Ieemdb.Web
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Bogus;

  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Models.Dto;

  public static class Fakers
  {
    public static readonly Faker<ActorDto> ActorProvider =
      new Faker<ActorDto>()
        .RuleFor(e => e.Name, f => f.Person.FullName)
        .RuleFor(e => e.BirthDate, f => f.Date.Past())
        .RuleFor(e => e.Bio, f => f.Lorem.Sentence(5, 20))
        .RuleFor(e => e.Id, f => f.IndexFaker);

    public static readonly Faker<Movie> MovieProvider =
      new Faker<Movie>();
  }
}
