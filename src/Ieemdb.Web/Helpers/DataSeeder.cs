namespace Esentis.Ieemdb.Web.Helpers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Persistence.Joins;

  using Microsoft.AspNetCore.Identity;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Storage;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Logging;

  internal static class DataSeeder
  {
    public static async Task SeedDatabase(this IServiceProvider services)
    {
      var logger = services.GetRequiredService<ILogger<IeemdbDbContext>>();
      using var scope = services.CreateScope();

      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IeemdbUser>>();
      var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IeemdbRole>>();
      var dbContext = scope.ServiceProvider.GetRequiredService<IeemdbDbContext>();

      if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
      {
        logger.LogError(LogTemplates.DatabaseIsMissingMigrations);
        return;
      }

      var config = services.GetRequiredService<IConfiguration>();

      logger.LogInformation(LogTemplates.SeedInit, nameof(IeemdbDbContext));

      await SeedRoles(roleManager);

      await SeedUsers(logger, userManager, config["Ieemdb:InitialAdminPassword"], config["Ieemdb:InitialAdminEmail"]);
    }

    private static async Task SeedRoles(RoleManager<IeemdbRole> roleManager)
    {
      var roles = await roleManager.Roles.ToListAsync();

      if (roles.All(x => x.Name != RoleNames.Administrator))
      {
        await roleManager.CreateAsync(new IeemdbRole { Name = RoleNames.Administrator, });
      }

      if (roles.All(x => x.Name != RoleNames.Member))
      {
        await roleManager.CreateAsync(new IeemdbRole
        {
          ConcurrencyStamp = "ed11f5d6-7eaf-4418-9f98-bcab656e16e0", Name = RoleNames.Member,
        });
      }
    }

    private static readonly Random Random = new Random();

    private static async Task SeedUsers(
      ILogger logger,
      UserManager<IeemdbUser> userManager,
      string adminInitialPassword,
      string adminInitialEmail)
    {
      var admins = await userManager.GetUsersInRoleAsync(RoleNames.Administrator);
      var user = new IeemdbUser
      {
        EmailConfirmed = true, Email = adminInitialEmail, UserName = "administrator", LockoutEnabled = false,
      };

      if (!admins.Any())
      {
        logger.LogInformation(LogTemplates.SeedAdmin, adminInitialEmail);
        var admin = await userManager.FindByEmailAsync(adminInitialEmail);

        var result = admin != null
          ? null
          : await userManager.CreateAsync(
            user,
            adminInitialPassword);

        if (admin != null || (result?.Succeeded ?? false))
        {
          admin ??= user;
          logger.LogInformation(LogTemplates.SeedAdmin, adminInitialEmail);
          if (!await userManager.IsInRoleAsync(admin, RoleNames.Administrator))
          {
            result = await userManager.AddToRoleAsync(admin, RoleNames.Administrator);
          }
        }
        else
        {
          logger.LogError(LogTemplates.SeedAdminFailed, result?.Errors);
        }
      }
    }
  }
}
