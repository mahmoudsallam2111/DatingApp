using DatingApp.Domain.Aggregates.AppUser.Entities;

namespace DatingApp.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokent(AppUser user);
    }
}
