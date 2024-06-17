using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Application.Interfaces.Users;

namespace DatingApp.Application.Features.Users
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;

        public UserAppService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<GetUserDto> GetUserById(long id)
        {
            var res = await _userRepository.GetByIdAsync(id);
            return new GetUserDto
            {
                Id = res.Id,
                Name = res.Name,
            };
        }
    }
}
