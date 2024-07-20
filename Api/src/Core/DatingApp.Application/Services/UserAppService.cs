using AutoMapper;
using BuildingBlocks.Exceptions;
using DatingApp.Application.Dtos;
using DatingApp.Application.Helpers;
using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Domain.Aggregates.AppUser.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserAppService(IUserRepository userRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<GetUserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                throw new NotFoundException("user is not found");
            return _mapper.Map<GetUserDto>(user);
        }

        public async Task<GetUserDto> GetUserByName(string name)
        {
            var user = await _userRepository.FindByUserName(name);
            if (user is null)
                throw new NotFoundException("user is not found");
            return _mapper.Map<GetUserDto>(user);
        }

        public async Task<PagesList<GetUserDto>> GetUsers(UserParams userParams)
        {
            var users = await _userRepository.GetAllAsync(userParams);
            return _mapper.Map<PagesList<GetUserDto>>(users);
        }

        public async Task<UserLoginDto?> LoginUser(LoginDto loginDto)
        {
            var user = await _userRepository.FindByUserName(loginDto.Name);

            if (user is null) return null;

            var correctPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!correctPassword)
            {
                return null;
            }

            return new UserLoginDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = await _tokenService.CreateTokent(user),
                PhotoUrl = user.Photos?.FirstOrDefault(p => p.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }

        public async Task<UserLoginDto> RegisterUser(RegisterUserDto registerUserDto)
        {
            if (await IsUserExist(registerUserDto.Name))
                throw new Exception("this Name is exist,U Can Register with anther one");


            Address address = new Address
            {
                Country = registerUserDto.Address.Country,
                City = registerUserDto.Address.City,
            };
            var userToRegitser = new AppUser
            {
                UserName = registerUserDto.Name,
                Gender = registerUserDto.Gender,
                KnownAs = registerUserDto.KnowAs,
                DateOfBirth = registerUserDto.DateOfBirth,
                Address = address,
            };

            var result = await _userManager.CreateAsync(userToRegitser , registerUserDto.Password);

            // assign the role member to registered user
            await _userManager.AddToRoleAsync(userToRegitser, "Member");

           
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    throw new Exception(error.Description.ToString());
                }
            }

            return new UserLoginDto
            {
                Id = userToRegitser.Id,
                UserName = userToRegitser.UserName,
                Token = await _tokenService.CreateTokent(userToRegitser),
                PhotoUrl = userToRegitser.Photos?.FirstOrDefault(p => p.IsMain)?.Url,
                KnownAs = userToRegitser.KnownAs,
                Gender = userToRegitser.Gender,
            };
        }

        public async Task UpdateUser(UserUpdateDto userUpdateDto)
        {
            var userToUpdate = await _userRepository.GetByIdAsync(userUpdateDto.Id);

            if (userToUpdate == null) throw new NotFoundException("user is not found");

            userToUpdate.Introduction = userUpdateDto.Introduction;
            userToUpdate.LookingFor = userUpdateDto.LookingFor;
            userToUpdate.Interests = userUpdateDto.Interests;
            userToUpdate.Address.City = userUpdateDto.City;
            userToUpdate.Address.Country = userUpdateDto.Country;

            _userRepository.Update(userToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<bool> IsUserExist(string userName)
        {
            var users = await _userRepository.GetAllWithoutPaginationAsync();
            return users.Any(u => u.UserName.ToLower() == userName.ToLower());
        }
    }
}
