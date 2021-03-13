using Ieemdb.Persistence.Helpers;
using Ieemdb.Persistence.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ieemdb.Persistence
{
    public class IeemdbDbContext : IdentityDbContext<IeemdbUser, IeemdbRole, Guid>
    {
        private static readonly DateTimeOffset SeededAt = DateTime.Parse("13/03/2021");

        public IeemdbDbContext(DbContextOptions<IeemdbDbContext> options)
            : base(options)
        {
        }

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
        }
    }
}
