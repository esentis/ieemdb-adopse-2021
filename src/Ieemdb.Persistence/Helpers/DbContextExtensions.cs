namespace Esentis.Ieemdb.Persistence.Helpers
{
  using System.Linq;

  using Esentis.Ieemdb.Persistence.Abstractions;

  using Microsoft.EntityFrameworkCore;

  public static class DbContextExtensions
  {
    public static IQueryable<TEntity> FullTextSearch<TEntity>(this IQueryable<TEntity> source, string query)
      where TEntity : ISearchable
      => source
        .Where(x => EF.Functions.ToTsVector("english", x.NormalizedSearch).Matches("Npgsql"));
  }
}
