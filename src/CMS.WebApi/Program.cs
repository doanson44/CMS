using System;
using System.Net;
using System.Threading.Tasks;
using CMS.Core.Data.Entites;
using CMS.Infrastructure.Data;
using CMS.Infrastructure.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CMS.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateWebHostBuilder(args).Build();

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

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .ConfigureAppConfiguration(SetupConfiguration)
            .UseSentry(options =>
            {
                options.Release = "e386dfd"; // Could be also the be like: 2.0 or however your version your app
                options.TracesSampleRate = 0.1;
                options.MaxBreadcrumbs = 200;

                // Set a proxy for outgoing HTTP connections
                options.HttpProxy = null;

                // Example: Disabling support to compressed responses:
                options.DecompressionMethods = DecompressionMethods.None;

                options.MaxQueueItems = 100;
                options.ShutdownTimeout = TimeSpan.FromSeconds(10);

                // Configures the root scope
                options.ConfigureScope(s => s.SetTag("Always sent", "this tag"));
            })
        ;

    private static void SetupConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder configBuilder)
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
