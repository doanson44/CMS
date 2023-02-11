using System;
using System.Linq;
using System.Reflection;
using CMS.Core.Data;
using CMS.Core.Data.Repositories;
using CMS.Core.Jobs.Interfaces;
using CMS.Core.Services;
using CMS.Core.Services.Implementations;
using CMS.Core.Services.Interfaces;
using CMS.Core.Settings;
using CMS.Infrastructure.Data;
using CMS.Infrastructure.Data.Repositories;
using Hangfire;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.AutoRegisterDi;

namespace CMS.Scheduler
{
    public class Startup
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // mail
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();

            // database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(() => new Microsoft.Data.SqlClient.SqlConnection(Configuration.GetConnectionString("Hangfire")), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(5),
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true,
                    CommandTimeout = TimeSpan.FromMinutes(5)
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer(options =>
            {
                options.WorkerCount = 1;
                options.Queues = new[] { "alpha", "beta", "default" };
            });

            ConfigureContainer(services);

            // configuration
            services.Configure<JwtTokenSetting>(Configuration.GetSection("Jwt"));

            //services.AddIdentity<User, IdentityRole<Guid>>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, IBackgroundJobClient backgroundJobs)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookiePolicy();

            var user = Configuration.GetSection("HangfireAuthentication:User").Value;
            var pass = Configuration.GetSection("HangfireAuthentication:Pass").Value;

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireCustomBasicAuthenticationFilter() {
                    User = user,
                    Pass = pass
                } },
                IgnoreAntiforgeryToken = true
            });

            backgroundJobs.Enqueue(() => Console.WriteLine("Hangfire Start!"));

            RecurringJob.AddOrUpdate<IWorkManagementDeadlineService>(j => j.Process(), "*/30 * * * *");

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }

        public void ConfigureContainer(IServiceCollection services)
        {
            services.RegisterAssemblyPublicNonGenericClasses(Assembly.Load("CMS.Infrastructure"))
                .Where(c => c.Name.EndsWith("Repository"))
                .AsPublicImplementedInterfaces();

            services.RegisterAssemblyPublicNonGenericClasses(Assembly.Load("CMS.Core"))
                 .Where(c => c.Name.EndsWith("Services") || c.Name.EndsWith("Service"))
                 .AsPublicImplementedInterfaces();

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
            services.AddHttpContextAccessor();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IContactService, ContactsService>();
            services.AddScoped<IWorkManagementDeadlineService, WorkManagementDeadlineService>();
            services.AddHttpContextAccessor();
        }
    }
}
