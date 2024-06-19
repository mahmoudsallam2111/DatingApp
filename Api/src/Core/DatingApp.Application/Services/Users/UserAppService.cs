using AutoMapper;
using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Application.Interfaces.Users;
using DatingApp.Domain.Entities;

namespace DatingApp.Application.Features.Users
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserAppService(IUserRepository userRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetUserDto> GetUserById(int id)
        {
            var res = await _userRepository.GetByIdAsync(id);
            return new GetUserDto
            {
                Id = res.Id,
                Name = res.Name,
            };
        }

        public async Task<IReadOnlyList<GetUserDto>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<AppUser>, IReadOnlyList<GetUserDto>>(users);
        }

        public async Task<GetUserDto?> LoginUser(LoginDto loginDto)
        {
            var user =await _userRepository.FindByUserName(loginDto.UserName);
            GetUserDto? getUserDto = null;
            if (user == null)
            {
                return getUserDto;
            }
            else
            {
                return new GetUserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    PasswordHash = user.PasswordHash,
                    PasswordSalt = user.PasswordSalt,
                    
                };

            }

        }

        public async Task<UserLoginDto> RegisterUser(string userName, byte[] passwoedHash, byte[] passwoedSalt)
        {
            var userToRegitser = new AppUser { Name = userName, PasswordHash = passwoedHash, PasswordSalt = passwoedSalt };

            if (await IsUserExist(userName))
                throw new Exception("this user Is exist");

            var user =  await _userRepository.AddAsync(userToRegitser);
            await _unitOfWork.SaveChangesAsync();
            return new UserLoginDto
            { 
              UserName = user.Name, 
              Token = _tokenService.CreateTokent(user.Name)
            };
        }



        private async Task<bool> IsUserExist(string userName)
        {
            var users =await _userRepository.GetAllAsync();
            return users.Any(u=>u.Name.ToLower() == userName.ToLower());
        }
    }
}
