namespace Esentis.Ieemdb.Persistence.Identity
{
  using System;

  using Kritikos.Configuration.Persistence.Abstractions;

  using Microsoft.AspNetCore.Identity;

  public class IeemdbRole : IdentityRole<Guid>, IEntity<Guid>, ITimestamped
    {
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}
