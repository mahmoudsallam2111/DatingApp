using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserPhoto> Photos { get; set; }
        public DbSet<UserLike> Likes { get; set; }


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
