﻿namespace Esentis.Ieemdb.Persistence.Models
{
  using System;

  using Esentis.Ieemdb.Persistence.Base;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class Image : Entity<long>, IAuditable<Guid>
    {
        public string Url { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid UpdatedBy { get; set; }
    }
}
