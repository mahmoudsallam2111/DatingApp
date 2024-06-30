namespace DatingApp.Domain.Common
{
    public abstract class AuditableBaseEntity : BaseEntity
    {
        public long? CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public long? LastModifiedBy { get; set; }
        public DateTime LastModified { get; set; }
    }
}
