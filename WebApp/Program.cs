using NetBanking.Infrastructure.Identity;
using NetBanking.Infrastructure.Shared;
using NetBanking.Core.Application;
using WebApp.Middlewares;
using Microsoft.AspNetCore.Identity;
using NetBanking.Infrastructure.Identity.Entities;
using NetBanking.Infrastructure.Identity.Seeds;
using NetBanking.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSession();
builder.Services.AddTransient<ValidateUserSession, ValidateUserSession>();
builder.Services.AddScoped<LoginAuthorize>();

builder.Services.ApplicationLayerRegistration(builder.Configuration);
builder.Services.IdentityLayerRegistration(builder.Configuration);
builder.Services.PersistenceLayerRegistration(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await DefaultRoles.SeedAsync(userManager, roleManager);
        await AdminUser.SeedAsync(userManager, roleManager);
        await ClientUser.SeedAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        throw;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/User/HasError");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
