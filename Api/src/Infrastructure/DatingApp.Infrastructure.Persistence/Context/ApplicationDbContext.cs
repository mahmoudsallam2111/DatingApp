using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Domain.Aggregates.Group.Entities;
using DatingApp.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DatingApp.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<AppUser , AppRole,int ,
        IdentityUserClaim<int> , IdentityUserRole<int>, IdentityUserLogin<int> , 
        IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserPhoto> Photos { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Domain.Aggregates.Group.Entities.Group>  Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //var userId = string.IsNullOrEmpty(authenticatedUser.UserId)
            //    ? Guid.Empty : Guid.Parse(authenticatedUser.UserId);

            var currentTime = DateTime.Now;

            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = currentTime;
                        entry.Entity.CreatedBy = 1000;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = currentTime;
                        entry.Entity.LastModifiedBy =1000;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder Builder)
        {
            Builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(Builder);
        }
    }
}
