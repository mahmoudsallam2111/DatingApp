using DatingApp.Domain.Common;

namespace DatingApp.Domain.Aggregates.AppUser.Entities
{
    public class UserPhoto : AuditableBaseEntity
    {
        public string Url { get; set; } = string.Empty;
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }
        public long AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
