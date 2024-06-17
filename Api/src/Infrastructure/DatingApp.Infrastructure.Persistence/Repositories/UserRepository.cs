using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Domain.Entities;
using DatingApp.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<AppUser> , IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
