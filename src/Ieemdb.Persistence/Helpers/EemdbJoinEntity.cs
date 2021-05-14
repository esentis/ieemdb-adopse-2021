namespace Esentis.Ieemdb.Persistence.Helpers
{
  using System;

  using Kritikos.Configuration.Persistence.Contracts.Behavioral;

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
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime UpdatedAt { get; set; }

    #endregion
  }
}
