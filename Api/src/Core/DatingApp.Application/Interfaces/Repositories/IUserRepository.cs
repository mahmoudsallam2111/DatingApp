using DatingApp.Application.Dtos;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
        Task<AppUser?> FindByUserName(string name);
    }
}
