using DatingApp.Application.Helpers;
using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<AppUser> , IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AppUser?> FindByUserName(string name)
        {
            return await _dbContext.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(u=>u.Name == name);
        }

        public override async Task<PagesList<AppUser>> GetAllAsync(UserParams userParams)
        {
            Expression<Func<AppUser , bool>> predicate = 
                appuser => appuser.Name != userParams.CurrentUser &&
                appuser.Gender == userParams.Gender &&
                appuser.Age > userParams.MinAge &&
                appuser.Age < userParams.MaxAge;

            var query = _dbContext.Users
                   .Include(u => u.Photos)
                   .Where(predicate)
                   .AsNoTracking();

          return await PagesList<AppUser>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);

        }

        public override async Task<AppUser> GetByIdAsync(int id)
        {
            return await _dbContext.Users
                .Include(u => u.Photos)
                .SingleAsync(u => u.Id == id);
        }
    }
}
