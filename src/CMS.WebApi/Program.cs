using System;
using System.Net;
using System.Threading.Tasks;
using CMS.Core.Data.Entites;
using CMS.Core.Exceptions;
using CMS.Infrastructure.Data;
using CMS.Infrastructure.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Sentry;

namespace CMS.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        try
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var identityContext = services.GetRequiredService<ApplicationDbContext>();
                await ApplicationDbContextSeed.SeedAsync(identityContext, userManager, roleManager);
            }
        }
        catch { }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(SetupConfiguration)
            .UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseSentry(options =>
                {
                    // The parameter 'options' here has values populated through the configuration system.
                    // That includes 'appsettings.json', environment variables and anything else
                    // defined on the ConfigurationBuilder.
                    // See: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.1&tabs=basicconfiguration
                    // Tracks the release which sent the event and enables more features: https://docs.sentry.io/learn/releases/
                    // If not explicitly set here, the SDK attempts to read it from: AssemblyInformationalVersionAttribute and AssemblyVersion
                    // TeamCity: %build.vcs.number%, VSTS: BUILD_SOURCEVERSION, Travis-CI: TRAVIS_COMMIT, AppVeyor: APPVEYOR_REPO_COMMIT, CircleCI: CIRCLE_SHA1
                    options.Release = "e386dfd"; // Could be also the be like: 2.0 or however your version your app
                    options.TracesSampleRate = 0.1;
                    options.MaxBreadcrumbs = 200;

                    // Set a proxy for outgoing HTTP connections
                    options.HttpProxy = null; // new WebProxy("https://localhost:3128");

                    // Example: Disabling support to compressed responses:
                    options.DecompressionMethods = DecompressionMethods.None;

                    options.MaxQueueItems = 100;
                    options.ShutdownTimeout = TimeSpan.FromSeconds(10);

                    options.AddExceptionFilterForType<TaskCanceledException>();
                    options.AddExceptionFilterForType<BusinessException>();

                    // Configures the root scope
                    options.ConfigureScope(s => s.SetTag("Always sent", "this tag"));
                });
                webBuilder.UseStartup<Startup>();
            });

    private static void SetupConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder configBuilder)
    {
        var env = hostingContext.HostingEnvironment;

        configBuilder.SetBasePath(env.ContentRootPath)
            .AddEnvironmentVariables();

        configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        configBuilder.AddEnvironmentVariables();

        configBuilder.Build();
    }
}
