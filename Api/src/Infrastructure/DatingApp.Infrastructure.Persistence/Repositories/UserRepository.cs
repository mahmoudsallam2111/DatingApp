using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Domain.Entities;
using DatingApp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return await _dbContext.Users.SingleOrDefaultAsync(u=>u.Name == name);
        }
    }
}
