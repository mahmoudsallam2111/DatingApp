using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Domain.Aggregates.AppUser.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Context.EntityConfiguration
{
    public class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.KnownAs).IsUnicode();

            builder.Property(u => u.DateOfBirth).IsRequired();

            builder.Property(u => u.Interests).IsRequired();

            builder.HasMany(u => u.Photos)
               .WithOne(p => p.AppUser)
               .HasForeignKey(p => p.AppUserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.ComplexProperty(u => u.Address, u =>
            {
                u.Property(a => a.Country).IsRequired();
                u.Property(a => a.City).IsRequired();
            });

                
                
        }
    }
}
