using DatingApp.Domain.Aggregates.AppUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Context.EntityConfiguration
{
    //public class AppUserRoleEntityTypeConfiguration : IEntityTypeConfiguration<AppUserRole>
    //{
    //    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    //    {
    //        // Configure composite primary key
    //        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

    //        // Configure relationship with AppUser
    //        builder.HasOne(ur => ur.User)
    //               .WithMany(u => u.UserRoles)
    //               .HasForeignKey(ur => ur.UserId)
    //               .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

    //        // Configure relationship with AppRole
    //        builder.HasOne(ur => ur.Role)
    //               .WithMany(r => r.UserRoles)
    //               .HasForeignKey(ur => ur.RoleId)
    //               .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
    //    }
    //}
}
