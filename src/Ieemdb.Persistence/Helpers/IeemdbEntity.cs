namespace Esentis.Ieemdb.Persistence.Helpers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  using Kritikos.Configuration.Persistence.Abstractions;

  public class IeemdbEntity<TKey> : IEntity<TKey>, ITimestamped, IAuditable<string>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
  {
    public TKey Id { get; set; }
    #region Implementation of ITimestamped

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; }

    #endregion
    #region Implementation of IAuditable<string>

    /// <inheritdoc />
    public string CreatedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    public string UpdatedBy { get; set; } = string.Empty;

    #endregion
  }
}
