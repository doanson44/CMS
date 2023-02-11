using CMS.Core.Data;
using CMS.Core.Data.Repositories;
using CMS.Core.Settings;
using CMS.Infrastructure.Data;
using CMS.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using System.Linq;
using System.Reflection;

namespace CMS.WebApi
{
    public static class DependencyConfig
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration, string rootPath)
        {
            // configuration
            services.Configure<JwtTokenSetting>(configuration.GetSection("Jwt"));
            services.Configure<ProductionTestingSetting>(configuration.GetSection(ProductionTestingSetting.ConfigKey));

            services.RegisterAssemblyPublicNonGenericClasses(Assembly.Load("CMS.Infrastructure"))
                .Where(c => c.Name.EndsWith("Repository"))
                .AsPublicImplementedInterfaces();

            services.RegisterAssemblyPublicNonGenericClasses(Assembly.Load("CMS.Core"))
                .Where(c => c.Name.EndsWith("Services") || c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces();

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();

        }
    }
}
