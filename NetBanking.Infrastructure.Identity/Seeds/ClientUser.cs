using Microsoft.AspNetCore.Identity;
using NetBanking.Core.Application.Enums;
using NetBanking.Infrastructure.Identity.Entities;

namespace NetBanking.Infrastructure.Identity.Seeds
{
    public class ClientUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            AppUser clientuser = new();
            clientuser.UserName = "clientuser";
            clientuser.Email = "clientuser@email.com";
            clientuser.FirstName = "Client";
            clientuser.LastName = "User";
            clientuser.PhoneNumber = "829-123-9811";
            clientuser.IdCard = "91-1981-1919";
            clientuser.IsActive = true;
            clientuser.EmailConfirmed = true;
            clientuser.PhoneNumberConfirmed = true;

            if (userManager.Users.All(u => u.Id != clientuser.Id))
            {
                var user = await userManager.FindByEmailAsync(clientuser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(clientuser, "123Pa$$word");
                    await userManager.AddToRoleAsync(clientuser, RolesEnum.Client.ToString());
                }
            }
        }
    }
}
