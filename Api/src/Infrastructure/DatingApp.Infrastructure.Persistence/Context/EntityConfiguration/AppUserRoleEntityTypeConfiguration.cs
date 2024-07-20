using DatingApp.Domain.Aggregates.AppUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Context.EntityConfiguration
{
    //public class AppUserRoleEntityTypeConfiguration : IEntityTypeConfiguration<AppUserRole>
    //{
    //    //public void Configure(EntityTypeBuilder<AppUserRole> builder)
    //    //{
    //    //    // Configure composite primary key
    //    //    builder.HasKey(ur => new { ur.UserId, ur.RoleId });

    //    //    // Configure relationship with AppUser
    //    //    builder.HasOne(ur => ur.User)
    //    //           .WithMany(u => u.UserRoles)
    //    //           .OnDelete(DeleteBehavior.Cascade); // Configure cascade delete as per your requirement

    //    //    // Configure relationship with AppRole
    //    //    builder.HasOne(ur => ur.Role)
    //    //           .WithMany(r => r.UserRoles)
    //    //           .OnDelete(DeleteBehavior.Cascade); // Configure cascade delete as per your requirement
    //    }
    //}
}
