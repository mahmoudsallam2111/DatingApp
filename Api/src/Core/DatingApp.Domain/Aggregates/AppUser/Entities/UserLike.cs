namespace DatingApp.Domain.Aggregates.AppUser.Entities;

public class UserLike
{
    public AppUser SourceUser { get; set; }
    public long SourceUserId { get; set; }
    public AppUser TargetUser { get; set; }
    public long TargetUserId { get; set; }

}
