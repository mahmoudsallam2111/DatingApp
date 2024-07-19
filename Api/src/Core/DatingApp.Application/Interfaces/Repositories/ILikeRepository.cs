using DatingApp.Application.Helpers;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Application.Interfaces.Repositories
{
    public interface ILikeRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId ,  int targetUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<IList<AppUser>> GetUserLikes(string predicate , int userId);
    }
}
