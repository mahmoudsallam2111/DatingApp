using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder Builder)
        {
            Builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(Builder);
        }
    }
}
