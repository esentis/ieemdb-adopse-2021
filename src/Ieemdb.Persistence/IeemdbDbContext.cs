namespace Esentis.Ieemdb.Persistence
{
  using System;

  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Persistence.Models;

  using Kritikos.Configuration.Peristence.IdentityServer;

  using Microsoft.EntityFrameworkCore;

  public class IeemdbDbContext : ApiAuthorizationPooledDbContext<IeemdbUser, IeemdbRole, Guid>
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

    public DbSet<Image> Images { get; set; }

    public DbSet<Movie> Movies { get; set; }

    public DbSet<Rating> Ratings { get; set; }

    public DbSet<Favorite> Favorites { get; set; }

    public DbSet<MovieActor> MovieActors { get; set; }

    public DbSet<MovieDirector> MovieDirectors { get; set; }

    public DbSet<MoviePoster> MoviePosters { get; set; }

    public DbSet<MovieScreenshot> MovieScreenshots { get; set; }

    public DbSet<MovieWriter> MovieWriters { get; set; }

    public DbSet<MovieGenre> MovieGenres { get; set; }

    // public DbSet<Device> Devices { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      builder.Entity<IeemdbRole>(entity =>
      {
        entity.HasData(new[]
              {
                    new IeemdbRole
                    {
                        CreatedAt = SeededAt,
                        UpdatedAt = SeededAt,
                        Id = Guid.Parse("bcb65d95-5cd1-4882-a1b5-f537cde80a22"),
                        ConcurrencyStamp = "e683bff6-ff91-4c1e-af8b-203cdcf0ba3c",
                        Name = RoleNames.Administrator,
                        NormalizedName = RoleNames.Administrator,
                    },
                    new IeemdbRole
                    {
                      CreatedAt = SeededAt,
                      UpdatedAt = SeededAt,
                      Id = Guid.Parse("7ac8f688-4a10-48c7-8b00-73c52dda15df"),
                      ConcurrencyStamp = "ed11f5d6-7eaf-4418-9f98-bcab656e16e0",
                      Name = RoleNames.Member,
                      NormalizedName = RoleNames.Member,
                    },
              });
      });

      builder.Entity<MovieActor>(e =>
      {
        e.HasOne(mv => mv.Movie)
                  .WithMany()
                  .HasForeignKey("MovieId").OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Actor)
                  .WithMany()
                  .HasForeignKey("ActorId").OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "ActorId");
      });

      builder.Entity<MovieDirector>(e =>
      {
        e.HasOne(mv => mv.Movie)
                  .WithMany()
                  .HasForeignKey("MovieId").OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Director)
                  .WithMany()
                  .HasForeignKey("DirectorId").OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "DirectorId");
      });

      builder.Entity<MovieWriter>(e =>
      {
        e.HasOne(mv => mv.Movie)
                  .WithMany()
                  .HasForeignKey("MovieId").OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Writer)
                  .WithMany()
                  .HasForeignKey("WriterId").OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "WriterId");
      });

      builder.Entity<MovieGenre>(e =>
      {
        e.HasOne(mv => mv.Movie)
                  .WithMany()
                  .HasForeignKey("MovieId").OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Genre)
                  .WithMany()
                  .HasForeignKey("GenreId").OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "GenreId");
      });

      builder.Entity<MoviePoster>(e =>
      {
        e.HasOne(mv => mv.Movie)
                  .WithMany()
                  .HasForeignKey("MovieId").OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Poster)
                  .WithMany()
                  .HasForeignKey("PosterId").OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "PosterId");
      });

      builder.Entity<MovieScreenshot>(e =>
      {
        e.HasOne(mv => mv.Movie)
                  .WithMany()
                  .HasForeignKey("MovieId").OnDelete(DeleteBehavior.Restrict);
        e.HasOne(mv => mv.Screenshot)
                  .WithMany()
                  .HasForeignKey("ScreenshotId").OnDelete(DeleteBehavior.Restrict);
        e.HasKey("MovieId", "ScreenshotId");
      });
    }
  }
}
