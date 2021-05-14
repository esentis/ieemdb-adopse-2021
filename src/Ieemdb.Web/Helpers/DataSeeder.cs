namespace Esentis.Ieemdb.Web.Helpers
{
  using System;
  using System.Linq;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Persistence.Models;
  using Esentis.Ieemdb.Web.Providers;

  using Microsoft.AspNetCore.Identity;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Logging;

  using Refit;

  internal static class DataSeeder
  {
    public static async Task SeedDatabase(this IServiceProvider services)
    {
      var logger = services.GetRequiredService<ILogger<IeemdbDbContext>>();
      using var scope = services.CreateScope();

      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IeemdbUser>>();
      var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IeemdbRole>>();
      var dbContext = scope.ServiceProvider.GetRequiredService<IeemdbDbContext>();
      var api = scope.ServiceProvider.GetRequiredService<ITheMovieDb>();

      if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
      {
        logger.LogError(LogTemplates.DatabaseIsMissingMigrations);
        return;
      }

      var config = services.GetRequiredService<IConfiguration>();

      logger.LogInformation(LogTemplates.SeedInit, nameof(IeemdbDbContext));

      await SeedRoles(roleManager);

      await SeedUsers(logger, userManager, config["Ieemdb:InitialAdminPassword"], config["Ieemdb:InitialAdminEmail"]);

      await SeedGenres(dbContext, api, logger);

      await SeedCountries(dbContext, api, logger);

      await dbContext.SaveChangesAsync();
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

    private static async Task SeedCountries(IeemdbDbContext ctx, ITheMovieDb api, ILogger logger)
    {
      if (await ctx.Countries.AnyAsync())
      {
        return;
      }

      try
      {
        var countries = await api.GetCountries();
        var countryEntities = countries.Select(x => new Country { Iso = x.iso_3166_1, Name = x.english_name })
          .ToList();
        ctx.Countries.AddRange(countryEntities);
      }
      catch (ApiException e)
      {
        logger.LogCritical(e, "Unhandled exception caught: {Message}", e.Message);
      }
    }

    private static async Task SeedGenres(IeemdbDbContext ctx, ITheMovieDb api, ILogger logger)
    {
      if (await ctx.Genres.AnyAsync())
      {
        return;
      }

      var genres = await api.GetGenres();
      var genreEntities = genres.genres.Select(x => new Genre { Name = x.name, TmdbId = x.id, }).ToList();

      ctx.Genres.AddRange(genreEntities);
    }

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
