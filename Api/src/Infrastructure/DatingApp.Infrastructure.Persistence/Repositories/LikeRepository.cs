using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Persistence.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LikeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _dbContext.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<IList<AppUser>> GetUserLikes(string predicate, int userId)
        {
            var likes = _dbContext.Likes.AsQueryable();

            if (predicate == "like")
            {
                likes = likes.Where(l => l.SourceUserId == userId);
            }
            else if (predicate == "likedBy")
            {
                likes = likes.Where(l => l.TargetUserId == userId);
            }

            var userIds = await likes.Select(l => predicate == "like" ? l.TargetUserId : l.SourceUserId).ToListAsync();

            var users = await _dbContext.Users
                .Where(u => userIds.Contains(u.Id))
                .Include(u => u.Photos)
                .AsSplitQuery()
                .OrderBy(u => u.UserName)
                .ToListAsync();

            return users;
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _dbContext.Users
                .AsSplitQuery()
                .Include(x=>x.LikedUsers)
                .FirstAsync(x=>x.Id == userId);
        }
    }
}
