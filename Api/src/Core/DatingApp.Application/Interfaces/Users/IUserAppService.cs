using DatingApp.Application.Dtos;

namespace DatingApp.Application.Interfaces.Users
{
    public interface IUserAppService
    {
       Task<GetUserDto> GetUserById(int id);
       Task<UserLoginDto> RegisterUser(string userName , byte[] passwoedHash , byte[] passwoedSalt);
       Task<GetUserDto?> LoginUser(LoginDto loginDto);
    }
}
