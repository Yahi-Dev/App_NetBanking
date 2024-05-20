using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Infrastructure.Identity.Entities;
using NetBanking.Infrastructure.Identity.Services;
using NetBanking.Infrastructure.Persistence.Contexts;
using NetBanking.Core.Application.Helpers;

namespace NetBanking.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void IdentityLayerRegistration(this IServiceCollection service, IConfiguration configuration)
        {
            #region Context
            if (configuration.GetValue<bool>("UserInMemoryDatabase"))
            {
                service.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("InMemoryIdentity"));
            }
            else
            {
                service.AddDbContext<IdentityContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                    m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
                });
            }
            #endregion

            #region Services
            
            
            service.AddIdentity<AppUser, IdentityRole>()
              .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            service.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User";
                options.AccessDeniedPath = "/User/AccessDenied";
            });

            service.AddAuthentication();
            
            #endregion

            service.AddTransient<IAccountService, AccountService>();
        }
    }
}
