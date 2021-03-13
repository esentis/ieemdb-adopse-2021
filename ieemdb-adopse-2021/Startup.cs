using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ieemdb_adopse_2021.Helpers;
using Kritikos.Configuration.Persistence.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Ieemdb.Persistence;
using Ieemdb.Persistence.Identity;
using ieemdb_adopse_2021.Options;
using Kritikos.Configuration.Persistence.Extensions;
using Kritikos.Configuration.Persistence.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ieemdb_adopse_2021
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        private IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var isDevelopment = Environment.IsDevelopment();

            services.AddControllers().AddControllersAsServices().AddViewComponentsAsServices().AddTagHelpersAsServices();
            services.AddMvc();
            services.AddControllersWithViews();
            services.Configure<JwtOptions>(options => Configuration.GetSection("JWT").Bind(options));

            services.AddHttpContextAccessor();
            services.AddScoped(sp => new AuditorProvider(sp.GetRequiredService<IHttpContextAccessor>()));
            services.AddScoped(sp => new AuditSaveChangesInterceptor<Guid>(sp.GetRequiredService<AuditorProvider>()));
            services.AddSingleton(sp => new TimestampSaveChangesInterceptor());

            services.AddDbContext<IeemdbDbContext>((serviceProvider, options) =>
            {
                // This is localhost connection string.
                options.UseNpgsql(
                        "Host=localhost;Username=postgres;Password=ad1e35c1368e4d298abae3a73f37a424;Database=ieemdb;Application Name=IEEMDB")
                    .AddInterceptors(
                        serviceProvider.GetRequiredService<TimestampSaveChangesInterceptor>(),
                        serviceProvider.GetRequiredService<AuditSaveChangesInterceptor<Guid>>())
                    .EnableCommonOptions(Environment);
            });

            services.AddHostedService<MigrationService<IeemdbDbContext>>();

            services.AddIdentity<IeemdbUser, IeemdbRole>(c =>
                {
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
                .AddDefaultTokenProviders();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ieemdb.Web.Api", Version = "v1" });

                c.DescribeAllParametersInCamelCase();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                /* c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                     $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));*/
            });

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
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ieemdb.Web.Api v1");
                    c.DocumentTitle = "Ieemdb API";
                    c.DocExpansion(DocExpansion.None);
                    c.EnableDeepLinking();
                    c.EnableFilter();
                    c.EnableValidator();
                    c.DisplayOperationId();
                    c.DisplayRequestDuration();
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
