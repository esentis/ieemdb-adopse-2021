﻿namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Base;
  using Esentis.Ieemdb.Persistence.Identity;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Favorite : Entity<long>, IAuditable<Guid>
    {
        public IeemdbUser User { get; set; }

        public Movie Movie { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid UpdatedBy { get; set; }
    }
}
