using Kritikos.Configuration.Persistence.Abstractions;
using System;

namespace Ieemdb.Persistence.Base
{
    public abstract class Entity<TKey> : IEntity<TKey>, ITimestamped
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public TKey Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}
