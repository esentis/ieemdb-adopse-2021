namespace Esentis.Ieemdb.Persistence.Helpers
{
  using System;

  using Kritikos.Configuration.Persistence.Abstractions;

  public abstract class EmdbKeylessEntity : IAuditable<Guid>, ITimestamped
  {
    #region Implementation of IAuditable<Guid>

    /// <inheritdoc />
    public Guid CreatedBy { get; set; }

    /// <inheritdoc />
    public Guid UpdatedBy { get; set; }

    #endregion
    #region Implementation of ITimestamped

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; }

    #endregion
  }
}
