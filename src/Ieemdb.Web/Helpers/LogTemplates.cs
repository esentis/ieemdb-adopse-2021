namespace Esentis.Ieemdb.Web.Helpers
{
  public class LogTemplates
  {
    public const string BootstrappingError = "Error while bootstrapping application: {Message}";

    public const string CreatedEntity = "Created {Entity} {@Value}";

    public const string RequestEntity = "User requested {Entity} with ID {Id}";

    public const string RequestEntities = "User requested collection of {Entity}. Found {Count} records";

    public const string Conflict = "{Entity} with ID {Id} has assignments";

    public const string NotFound = "{Entity} not found";

    public const string Deleted = "{Entity} with ID {Id} has been deleted";

    public const string Updated = "{Entity} has been updated";
  }
}
