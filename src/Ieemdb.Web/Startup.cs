namespace Esentis.Ieemdb.Web
{
  using System;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Web.Helpers;

  using Kritikos.Configuration.Persistence.Extensions;
  using Kritikos.Configuration.Persistence.Interceptors;
  using Kritikos.Configuration.Persistence.Services;
  using Kritikos.PureMap;
  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authentication;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;

  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      Configuration = configuration;
      Environment = environment;
    }

    public IConfiguration Configuration { get; }

    public IWebHostEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<TimestampSaveChangesInterceptor>();
      services.AddHttpContextAccessor();
      services.AddSingleton<AuditSaveChangesInterceptor<Guid>>();
      services.AddSingleton<IAuditorProvider<Guid>>(sp =>
        new AuditorProvider(sp.GetRequiredService<IHttpContextAccessor>()));

      services.AddDbContextPool<IeemdbDbContext>((serviceProvider, options) =>
      {
        // This is localhost connection string.
        options.UseNpgsql(
            Configuration.GetConnectionString("Ieemdb"))
          .AddInterceptors(
            serviceProvider.GetRequiredService<TimestampSaveChangesInterceptor>(),
            serviceProvider.GetRequiredService<AuditSaveChangesInterceptor<Guid>>())
          .EnableCommonOptions(Environment);
      });

      services.AddDatabaseDeveloperPageExceptionFilter();
      services.AddHostedService<MigrationService<IeemdbDbContext>>();
      services.AddSingleton<IPureMapper>(sp => new PureMapper(MappingConfiguration.Mapping));

      services.AddIdentity<IeemdbUser, IeemdbRole>(c =>
        {
          var isDevelopment = Environment.IsDevelopment();
          c.User.RequireUniqueEmail = !isDevelopment;
          c.Password = new PasswordOptions
          {
            RequireDigit = !isDevelopment,
            RequireLowercase = !isDevelopment,
            RequireUppercase = !isDevelopment,
            RequireNonAlphanumeric = !isDevelopment,
            RequiredLength = isDevelopment
              ? 4
              : 12,
          };
        })
        .AddEntityFrameworkStores<IeemdbDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

      services.AddIdentityServer()
        .AddApiAuthorization<IeemdbUser, IeemdbDbContext>();

      services.AddAuthentication()
        .AddIdentityServerJwt();

      services.AddControllersWithViews();
      services.AddRazorPages();

      services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });
    }

    public void Configure(IApplicationBuilder app)
    {
      if (Environment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSpaStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseIdentityServer();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
      });

      app.UseSpa(spa =>
      {
        spa.Options.SourcePath = "ClientApp";

        if (Environment.IsDevelopment())
        {
          spa.UseReactDevelopmentServer(npmScript: "start");
        }
      });
    }
  }
}
