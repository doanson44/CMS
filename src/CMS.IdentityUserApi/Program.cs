using CMS.IdentityUserLib;
using CMS.IdentityUserLib.Identity;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<ITokenClaimsService, IdentityTokenClaimService>();
Dependencies.ConfigureServices(builder.Configuration, builder.Services);
var app = builder.Build();


try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppIdentityDbContext>();

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var identityContext = services.GetRequiredService<AppIdentityDbContext>();
        await AppIdentityDbContextSeed.SeedAsync(identityContext, userManager, roleManager);
    }
}
catch { }

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthorizeServices();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
