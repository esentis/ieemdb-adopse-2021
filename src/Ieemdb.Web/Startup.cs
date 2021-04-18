namespace Esentis.Ieemdb.Web
{
  using System;
  using System.IdentityModel.Tokens.Jwt;
  using System.IO;
  using System.Reflection;
  using System.Text;

  using Esentis.Ieemdb.Persistence;
  using Esentis.Ieemdb.Persistence.Identity;
  using Esentis.Ieemdb.Web.Helpers;
  using Esentis.Ieemdb.Web.Helpers.Extensions;
  using Esentis.Ieemdb.Web.Options;

  using Kritikos.Configuration.Persistence.Extensions;
  using Kritikos.Configuration.Persistence.Interceptors;
  using Kritikos.Configuration.Persistence.Services;
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
      services.AddSingleton<IPureMapper>(sp => new PureMapper(MappingConfiguration.Mapping));
      services.AddApplicationInsightsTelemetry();

      services.AddHttpContextAccessor();
      services.AddSingleton<TimestampSaveChangesInterceptor>();
      services.AddSingleton<AuditSaveChangesInterceptor<Guid>>();

      services.AddSingleton<IAuditorProvider<Guid>>(sp =>
        new AuditorProvider(sp.GetRequiredService<IHttpContextAccessor>()));

      services.AddDbContextPool<IeemdbDbContext>((serviceProvider, options) =>
      {
        options.UseNpgsql(
            Configuration.GetConnectionString("Ieemdb"))
          .AddInterceptors(
            serviceProvider.GetRequiredService<TimestampSaveChangesInterceptor>(),
            serviceProvider.GetRequiredService<AuditSaveChangesInterceptor<Guid>>())
          .EnableCommonOptions(Environment);
      });

      services.AddDatabaseDeveloperPageExceptionFilter();
      services.AddHostedService<MigrationService<IeemdbDbContext>>();

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

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "IeemDB.Web.Api", Version = "v1" });
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

      services.AddCors(options =>
      {
        options.AddPolicy("AllowAll",
          builder =>
          {
            builder.AllowAnyOrigin();
          });
      });
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
      app.UseCors();
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
