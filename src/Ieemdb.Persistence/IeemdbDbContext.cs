#nullable disable // Not required for DbSet
namespace Esentis.Ieemdb.Persistence
{
  using System;

  using Esentis.Ieemdb.Persistence.Helpers;
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

    public DbSet<Actor> Actors { get; set; }

    public DbSet<Director> Directors { get; set; }

    public DbSet<Writer> Writers { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<Movie> Movies { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<Rating> Ratings { get; set; }

    public DbSet<Favorite> Favorites { get; set; }

    public DbSet<MovieActor> MovieActors { get; set; }

    public DbSet<MovieDirector> MovieDirectors { get; set; }

    public DbSet<MovieWatchlist> MovieWatchlists { get; set; }

    public DbSet<Poster> Posters { get; set; }

    public DbSet<Screenshot> Screenshots { get; set; }

    public DbSet<MovieWriter> MovieWriters { get; set; }

    public DbSet<MovieGenre> MovieGenres { get; set; }

    public DbSet<MovieCountry> MovieCountries { get; set; }

    public DbSet<Device> Devices { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      builder.Entity<Actor>()
        .HasIndex(e => e.NormalizedSearch)
        .IsTsVectorExpressionIndex("english");

      builder.Entity<Director>()
        .HasIndex(e => e.NormalizedSearch)
        .IsTsVectorExpressionIndex("english");

      builder.Entity<Movie>()
        .HasIndex(e => e.NormalizedTitle)
        .IsTsVectorExpressionIndex("english");

      builder.Entity<Movie>()
        .HasIndex(e => e.NormalizedSearch)
        .IsTsVectorExpressionIndex("english");

      builder.Entity<Writer>()
        .HasIndex(e => e.NormalizedSearch)
        .IsTsVectorExpressionIndex("english");

      builder.Entity<MovieActor>(e =>
      {
        e.HasOne(mv => mv.Movie)
          .WithMany()
          .HasForeignKey("MovieId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Actor)
          .WithMany()
          .HasForeignKey("ActorId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "ActorId");
      });

      builder.Entity<MovieDirector>(e =>
      {
        e.HasOne(mv => mv.Movie)
          .WithMany()
          .HasForeignKey("MovieId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Director)
          .WithMany()
          .HasForeignKey("DirectorId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "DirectorId");
      });

      builder.Entity<MovieWatchlist>(e =>
      {
        e.HasOne(mv => mv.Movie)
          .WithMany()
          .HasForeignKey("MovieId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Watchlist)
          .WithMany()
          .HasForeignKey("WatchlistId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "WatchlistId");
      });

      builder.Entity<MovieWriter>(e =>
      {
        e.HasOne(mv => mv.Movie)
          .WithMany()
          .HasForeignKey("MovieId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Writer)
          .WithMany()
          .HasForeignKey("WriterId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "WriterId");
      });

      builder.Entity<MovieGenre>(e =>
      {
        e.HasOne(mv => mv.Movie)
          .WithMany()
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
          .WithMany()
          .HasForeignKey("MovieId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Country)
          .WithMany()
          .HasForeignKey("CountryId")
          .OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "CountryId");
      });
    }
  }
}
