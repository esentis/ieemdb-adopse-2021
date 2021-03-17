namespace Esentis.Ieemdb.Persistence.Base
{
  using System;

  using Kritikos.Configuration.Persistence.Abstractions;

  public abstract class Entity<TKey> : IEntity<TKey>, ITimestamped
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public TKey Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}
