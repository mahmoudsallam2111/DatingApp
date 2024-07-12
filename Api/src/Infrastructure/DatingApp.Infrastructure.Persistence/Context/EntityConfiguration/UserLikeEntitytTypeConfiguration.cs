using DatingApp.Domain.Aggregates.AppUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Context.EntityConfiguration
{
    public class UserLikeEntitytTypeConfiguration : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> builder)
        {
            builder.HasKey(l=>new {l.SourceUserId , l.TargetUserId });  // composite key

            builder.HasOne(u => u.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(u => u.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.TargetUser)
                 .WithMany(l => l.LikedByUsers)
                 .HasForeignKey(u => u.TargetUserId)
                 .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
