namespace Esentis.Ieemdb.Web.Helpers
{
  public static class LogTemplates
  {
    public const string BootstrappingError = "Error while bootstrapping application: {Message}";

    public const string CreatedEntity = "Created {Entity} {@Value}";

    public const string RequestEntity = "User requested {Entity} with ID {Id}";

    public const string RequestEntities = "User requested collection of {Entity}. Found {Count} records";

    public const string Conflict = "{Entity} with ID {Id} has assignments";

    public const string NotFound = "{Entity} not found";

    public const string Deleted = "{Entity} with ID {Id} has been deleted";

    public const string Updated = "{Entity} has been updated";

    public const string SeedInit = "Seeding database  {Database}";

    public const string SeedAdmin = "Seeding new admin user  {AdminMail}";

    public const string SeedAdminFailed = "Failed seeding admin user with errors {Errors}";

    public const string DatabaseIsMissingMigrations = "Can not seed while database is missing migrations";
  }
}
