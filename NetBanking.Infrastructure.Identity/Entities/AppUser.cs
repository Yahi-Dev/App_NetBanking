using Microsoft.AspNetCore.Identity;

namespace NetBanking.Infrastructure.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdCard { get; set; }
        public bool IsActive { get; set; }
        public string? ImageURL { get; set; }
    }
}
