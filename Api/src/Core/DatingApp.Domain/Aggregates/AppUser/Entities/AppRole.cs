using Microsoft.AspNetCore.Identity;

namespace DatingApp.Domain.Aggregates.AppUser.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUser> UserRoles { get; set; } = new List<AppUser>();
    }
}
