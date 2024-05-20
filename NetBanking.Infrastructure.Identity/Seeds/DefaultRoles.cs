using Microsoft.AspNetCore.Identity;
using NetBanking.Core.Application.Enums;
using NetBanking.Infrastructure.Identity.Entities;

namespace NetBanking.Infrastructure.Identity.Seeds
{
    public class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(RolesEnum.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RolesEnum.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RolesEnum.Client.ToString()));
        }
    }
}
