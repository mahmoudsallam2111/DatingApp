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
        Task<UserLike> GetUserLike(long sourceUserId ,  long targetUserId);
        Task<AppUser> GetUserWithLikes(long userId);
        Task<IList<AppUser>> GetUserLikes(string predicate , long userId);
    }
}
