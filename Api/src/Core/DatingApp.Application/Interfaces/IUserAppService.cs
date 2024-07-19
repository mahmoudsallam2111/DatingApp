using DatingApp.Application.Dtos;
using DatingApp.Application.Helpers;

namespace DatingApp.Application.Interfaces
{
    public interface IUserAppService
    {
        Task<GetUserDto> GetUserById(int id);
        Task<GetUserDto> GetUserByName(string name);
        Task<PagesList<GetUserDto>> GetUsers(UserParams userParams);
        Task<UserLoginDto> RegisterUser(RegisterUserDto registerUserDto);
        Task<UserLoginDto?> LoginUser(LoginDto loginDto);
        Task UpdateUser(UserUpdateDto userUpdateDto);
    }
}
