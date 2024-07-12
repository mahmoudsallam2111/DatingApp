using BuildingBlocks.Helpers;
using DatingApp.Domain.Aggregates.AppUser.ValueObjects;
using DatingApp.Domain.Common;

namespace DatingApp.Domain.Aggregates.AppUser.Entities
{
    public class AppUser : AuditableBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; } = string.Empty;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; } = string.Empty;
        public string Introduction { get; set; } = string.Empty;
        public string LookingFor { get; set; } = string.Empty;
        public string Interests { get; set; } = string.Empty;
        public Address Address { get; set; } = new();
        public List<UserPhoto> Photos { get; set; } = new();

        public List<UserLike> LikedByUsers { get; set; } = new();  // users that like that a specific user
        public List<UserLike> LikedUsers { get; set; } = new();   // users that like that a specific user
        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }

        
    }
}
