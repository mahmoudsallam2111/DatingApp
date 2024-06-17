using DatingApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Infrastructure.Persistence.Context
{
    /// <summary>
    /// this class provide a centralize point to save changes to database as a unit of work
    /// </summary>
    /// <param name="dbContext"></param>
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        public async Task<bool> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync() > 0;
        }
    }
}
