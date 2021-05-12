namespace Esentis.Ieemdb.Persistence.Helpers
{
  using System;

  using Kritikos.Configuration.Persistence.Contracts.Behavioral;

  public class EemdbEntity<TKey> : EmdbKeylessEntity, IEntity<TKey>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
  {
    public TKey Id { get; set; }
  }
}
