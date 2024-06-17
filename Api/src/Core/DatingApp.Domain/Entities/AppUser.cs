using DatingApp.Domain.Common;

namespace DatingApp.Domain.Entities
{
    public class AppUser : AuditableBaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
