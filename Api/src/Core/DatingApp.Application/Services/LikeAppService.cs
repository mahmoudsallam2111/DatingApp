using AutoMapper;
using BuildingBlocks.Exceptions;
using DatingApp.Application.Dtos;
using DatingApp.Application.Helpers;
using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Repositories;

namespace DatingApp.Application.Services
{
    public class LikeAppService : ILikeAppService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LikeAppService(ILikeRepository likeRepository , 
            IUserRepository userRepository ,
            IUnitOfWork unitOfWork , 
            IMapper mapper)
        {
            _likeRepository = likeRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddLikeAsync(long sourceUserId, string likedUserName)
        {
            var likedUser = await _userRepository.FindByUserName(likedUserName);

            if (likedUser == null)  throw new NotFoundException("the liked user is not exist"); 

            var sourceUser = await _likeRepository.GetUserWithLikes(sourceUserId);
            if (sourceUser.Name == likedUserName) throw new Exception("You can not like yourself");

            var userLike = await _likeRepository.GetUserLike(sourceUserId , likedUser.Id);

            if (userLike != null) throw new Exception("You already liked this user");

            userLike = new Domain.Aggregates.AppUser.Entities.UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = likedUser.Id,
            };

            sourceUser.LikedUsers.Add(userLike);    

            await _unitOfWork.SaveChangesAsync();   

        }

        public async Task<List<LikeDto>> GetUserLikeAsync(string predicate, long userId)
        {
            var users = await _likeRepository.GetUserLikes(predicate , userId);

            var likeDtos = users.Select(user => new LikeDto
            {
                Id = user.Id,
                Name = user.Name,
                KnownAs = user.KnownAs,
                Age = user.Age,
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url
            }).ToList();

            return likeDtos;

        }
    }
}
