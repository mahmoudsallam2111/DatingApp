using DatingApp.Application.Dtos;

namespace DatingApp.Application.Interfaces.Users
{
    public interface IUserAppService
    {
       Task<GetUserDto> GetUserById(long id);
    }
}
