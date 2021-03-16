﻿using Ieemdb.Persistence.Helpers;
using Ieemdb.Persistence.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Ieemdb.Persistence.Models;

namespace Ieemdb.Persistence
{
    public class IeemdbDbContext : IdentityDbContext<IeemdbUser, IeemdbRole, Guid>
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
