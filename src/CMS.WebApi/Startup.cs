using System.Collections.Generic;
using System.Text;
using CMS.Core.Constants;
using CMS.Core.Data.Entites;
using CMS.Core.Settings;
using CMS.Infrastructure.Data;
using CMS.WebApi.Middleware;
using CMS.WebApi.Permission;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CMS.WebApi;

public class Startup
{
    public const string CORS_POLICY = "CorsPolicy";
    public IConfiguration Configuration { get; }
    private readonly IWebHostEnvironment hostingEnvironment;
    private readonly ILoggerFactory loggerFactory;

    public Startup(
        IConfiguration configuration,
        IWebHostEnvironment hostingEnvironment,
        ILoggerFactory loggerFactory)
    {
        Configuration = configuration;
        this.hostingEnvironment = hostingEnvironment;
        this.loggerFactory = loggerFactory;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        // mail
        services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        // database
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                x => x.UseNetTopologySuite());

            if (hostingEnvironment.IsProduction())
            {
                options.UseLoggerFactory(loggerFactory);
            }
        });

        services.Configure<MultipleDatabaseSettings>(Configuration.GetSection(nameof(MultipleDatabaseSettings)));
        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);
        services.AddAuthentication(config =>
        {
            config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(config =>
        {
            config.RequireHttpsMetadata = false;
            config.SaveToken = true;
            config.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        // swagger
        // Register the Swagger generator, defining one or more Swagger documents
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CMS API",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
              });
        });

        services.AddDependencies(Configuration, hostingEnvironment.ContentRootPath);
        services.AddAuthorizationCore();

        services.AddMemoryCache(options =>
        {
            options.SizeLimit = 1024;
        });

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }
        app.UseErrorHandler(loggerFactory);
        app.UseErrorLogging();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // enable swagger
        app.UseSwagger(c =>
        {
            c.SerializeAsV2 = true;
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMS API v1");
            c.RoutePrefix = string.Empty;
        });

        app.UseRouting();

        app.UseCors(CORS_POLICY);
        app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("content-disposition"));


        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
