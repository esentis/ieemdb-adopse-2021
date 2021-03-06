namespace Esentis.Ieemdb.Web
{
  using System;
  using System.IdentityModel.Tokens.Jwt;
  using System.IO;
  using System.Reflection;
  using System.Text;
  using System.Threading.Tasks;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Helpers;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Helpers.Extensions;
  using Esentis.Ieemdb.Web.Options;
  using Esentis.Ieemdb.Web.Providers;
  using Esentis.Ieemdb.Web.Services;

  using Kritikos.Configuration.Persistence.Extensions;
  using Kritikos.Configuration.Persistence.Interceptors.SaveChanges;
  using Kritikos.Configuration.Persistence.Interceptors.Services;
  using Kritikos.PureMap;
  using Kritikos.PureMap.Contracts;

  using Microsoft.AspNetCore.Authentication;
  using Microsoft.AspNetCore.Authentication.JwtBearer;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.UI.Services;
  using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Microsoft.IdentityModel.Tokens;
  using Microsoft.OpenApi.Models;

  using Refit;

  using Serilog;

  using Swashbuckle.AspNetCore.Filters;
  using Swashbuckle.AspNetCore.SwaggerUI;

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
      if (string.IsNullOrEmpty(Configuration["ApiKeys:TMDB"]))
      {
        services.AddSingleton<ITheMovieDb, FakeMovieDb>();
      }
      else
      {
        var settings = new RefitSettings
        {
          AuthorizationHeaderValueGetter = () => Task.FromResult(Configuration["ApiKeys:TMDB"]),
        };
        services.AddSingleton(sp => RestService.For<ITheMovieDb>("https://api.themoviedb.org/3", settings));
      }

      services.AddHostedService<MovieSyncingService>();
      services.AddHostedService<DeletedCleanupService>();
      services.AddHostedService<RefreshTokenCleanupService>();
      services.AddSingleton<IPureMapper>(sp => new PureMapper(MappingConfiguration.Mapping));
      services.AddApplicationInsightsTelemetry();

      services.AddHttpContextAccessor();
      services.AddScoped<RazorViewToStringRenderer>();
      services.AddSingleton<TimestampSaveChangesInterceptor>();
      services.AddSingleton<AuditSaveChangesInterceptor<Guid>>();

      services.AddSingleton<IAuditorProvider<Guid>>(sp =>
        new AuditorProvider(sp.GetRequiredService<IHttpContextAccessor>()));

      services.AddDbContextPool<IeemdbDbContext>((serviceProvider, options) =>
      {
        options.UseNpgsql(
            Configuration.GetConnectionString("Ieemdb"), npgsql => npgsql
              .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1), Array.Empty<string>())
              .UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
          .AddInterceptors(
            serviceProvider.GetRequiredService<TimestampSaveChangesInterceptor>(),
            serviceProvider.GetRequiredService<AuditSaveChangesInterceptor<Guid>>())
          .EnableCommonOptions(Environment);
      });

      services.AddDatabaseDeveloperPageExceptionFilter();

      var sendgrid = Configuration.GetValue<string>("SendGrid:ApiKey");
      if (string.IsNullOrEmpty(sendgrid))
      {
        services.AddSingleton<IEmailSender, DummyEmailSender>();
      }
      else
      {
        services.AddSingleton<IEmailSender, SendGridEmailSender>(sp => new SendGridEmailSender(sendgrid));
      }

      services.Configure<JwtOptions>(options => Configuration.GetSection("JWT").Bind(options));
      services.Configure<ServiceDurations>(options => Configuration.GetSection("ServiceDurations").Bind(options));

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "IEEMDB.Web.Api", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer",
          BearerFormat = "JWT",
          In = ParameterLocation.Header,
          Description =
            "Enter 'Bearer' [space] and then your valid Token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer", },
            },
            Array.Empty<string>()
          },
        });
        c.DescribeAllParametersInCamelCase();
        c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
        c.IncludeXmlComments(Path.Combine(
          AppContext.BaseDirectory,
          $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
      });

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
              : 6,
          };
        })
        .AddEntityFrameworkStores<IeemdbDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

      services.AddIdentityServer()
        .AddApiAuthorization<IeemdbUser, IeemdbDbContext>();

      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
      services.AddAuthentication(options =>
        {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
          options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = Configuration["JWT:Audience"],
            ValidIssuer = Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"])),
          };
        })
        .AddIdentityServerJwt();

      services.AddCorrelation();
      services.AddControllersWithViews();
      services.AddRazorPages();

      services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseSwagger();
      if (Environment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "IeemDB.Web.Api v1");
          c.DocumentTitle = "IeemDB API";
          c.DocExpansion(DocExpansion.None);
          c.EnableDeepLinking();
          c.EnableFilter();
          c.EnableValidator();
          c.DisplayOperationId();
          c.DisplayRequestDuration();
        });
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
      app.UseReDoc(c =>
      {
        c.RoutePrefix = "docs";
        c.DocumentTitle = "Ieemdb API Documentation v1";
        c.SpecUrl("/swagger/v1/swagger.json");
        c.ExpandResponses("none");
        c.RequiredPropsFirst();
        c.SortPropsAlphabetically();
        c.HideDownloadButton();
        c.HideHostname();
      });

      app.UseSerilogIngestion(x =>
      {
        x.ClientLevelSwitch = Program.LevelSwitch;
      });

      app.UseAuthentication();
      app.UseIdentityServer();
      app.UseAuthorization();

      app.UseCorrelation();

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

        if (Environment.IsDevelopment() && !Configuration.GetValue("AzureDeployment", false))
        {
          spa.UseReactDevelopmentServer(npmScript: "start");
        }
      });
    }
  }
}
