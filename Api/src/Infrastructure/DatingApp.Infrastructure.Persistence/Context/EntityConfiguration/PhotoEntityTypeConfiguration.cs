using DatingApp.Domain.Aggregates.AppUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Context.EntityConfiguration
{
    public class PhotoEntityTypeConfiguration : IEntityTypeConfiguration<UserPhoto>
    {
        public void Configure(EntityTypeBuilder<UserPhoto> builder)
        {
            builder.HasKey(p=>p.Id);
            builder.Property(p=>p.Url).IsRequired();

            builder.Property(p=>p.IsMain).IsRequired();

            builder.Property(p => p.PublicId).IsRequired(false);

            builder.Property(p => p.AppUserId)
                .HasColumnName("UserId");

            builder.HasOne(p => p.AppUser)
                .WithMany(u => u.Photos)
                .HasForeignKey(p => p.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
