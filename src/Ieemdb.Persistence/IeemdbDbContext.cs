#nullable disable // Not required for DbSet
namespace Esentis.Ieemdb.Persistence
{
  using System;

  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Persistence.Joins;
  using Esentis.Ieemdb.Persistence.Models;

  using Kritikos.Configuration.Peristence.IdentityServer;

  using Microsoft.EntityFrameworkCore;

  public class IeemdbDbContext : ApiAuthorizationDbContext<IeemdbUser, IeemdbRole, Guid>
  {
    private static readonly DateTimeOffset SeededAt = DateTime.Parse("13/03/2021");

    public IeemdbDbContext(DbContextOptions<IeemdbDbContext> options)
      : base(options)
    {
    }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<Movie> Movies { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<Watchlist> Watchlists { get; set; }

    public DbSet<Rating> Ratings { get; set; }

    public DbSet<Image> Images { get; set; }

    public DbSet<MovieGenre> MovieGenres { get; set; }

    public DbSet<MovieCountry> MovieCountries { get; set; }

    public DbSet<Device> Devices { get; set; }

    public DbSet<Favorite> Favorites { get; set; }

    public DbSet<Person> People { get; set; }

    public DbSet<MoviePerson> MoviePeople { get; set; }

    public DbSet<Video> Videos { get; set; }

    public DbSet<ServiceBatchingProgress> ServiceBatchingProgresses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<Person>()
        .HasQueryFilter(x => !x.IsDeleted)
        .HasIndex(e => e.NormalizedSearch)
        .IsTsVectorExpressionIndex("english");

      builder.Entity<Video>()
        .HasQueryFilter(x => !x.IsDeleted);

      builder.Entity<Image>()
        .HasQueryFilter(x => !x.IsDeleted);

      builder.Entity<Movie>()
        .HasQueryFilter(x => !x.IsDeleted)
        .HasIndex(e => e.NormalizedTitle)
        .IsTsVectorExpressionIndex("english");

      builder.Entity<Movie>()
        .HasIndex(e => e.NormalizedSearch)
        .IsTsVectorExpressionIndex("english");


      builder.Entity<MoviePerson>(e =>
      {
        e.HasOne(mv => mv.Movie)
          .WithMany(x => x.People)
          .HasForeignKey("MovieId")
          .OnDelete(DeleteBehavior.Restrict)
          .IsRequired();
        e.HasOne(mv => mv.Person)
          .WithMany()
          .HasForeignKey("PersonId")
          .OnDelete(DeleteBehavior.Restrict)
          .IsRequired();
        e.HasKey("MovieId", "PersonId");
      });

      builder.Entity<Genre>(e =>
      {
        e.HasQueryFilter(x => !x.IsDeleted);
      });

      builder.Entity<Country>(e =>
      {
        e.HasQueryFilter(x => !x.IsDeleted);
      });

      builder.Entity<MovieGenre>(e =>
      {
        e.HasOne(mv => mv.Movie)
          .WithMany(x => x.MovieGenres)
          .HasForeignKey("MovieId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Genre)
          .WithMany()
          .HasForeignKey("GenreId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "GenreId");
      });

      builder.Entity<MovieCountry>(e =>
      {
        e.HasOne(mv => mv.Movie)
          .WithMany(x => x.MovieCountries)
          .HasForeignKey("MovieId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Country)
          .WithMany()
          .HasForeignKey("CountryId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "CountryId");
      });

      builder.Entity<Rating>(e =>
      {
        e.HasOne(mv => mv.Movie)
          .WithMany(x => x.Ratings)
          .OnDelete(DeleteBehavior.Restrict);
      });

      builder.Entity<Image>(e =>
      {
        e.HasOne(mv => mv.Movie)
          .WithMany(x => x.Images)
          .OnDelete(DeleteBehavior.Restrict);
      });

      builder.Entity<Video>(e =>
      {
        e.HasOne(mv => mv.Movie)
          .WithMany(x => x.Videos)
          .OnDelete(DeleteBehavior.Restrict);
      });
    }
  }
}
