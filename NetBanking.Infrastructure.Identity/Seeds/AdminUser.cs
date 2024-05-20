using Microsoft.AspNetCore.Identity;
using NetBanking.Core.Application.Enums;
using NetBanking.Infrastructure.Identity.Entities;

namespace NetBanking.Infrastructure.Identity.Seeds
{
    public class AdminUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            AppUser adminuser = new();
            adminuser.UserName = "adminuser";
            adminuser.Email = "adminuser@email.com";
            adminuser.FirstName = "admin";
            adminuser.LastName = "user";
            adminuser.PhoneNumber = "829-123-9811";
            adminuser.IdCard = "91-1981-1919";
            adminuser.IsActive = true;
            adminuser.EmailConfirmed = true;
            adminuser.PhoneNumberConfirmed = true;

            if (userManager.Users.All(u => u.Id != adminuser.Id))
            {
                var user = await userManager.FindByEmailAsync(adminuser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(adminuser, "123Pa$$word");
                    await userManager.AddToRoleAsync(adminuser, RolesEnum.Admin.ToString());
                }
            }
        }
    }
}
