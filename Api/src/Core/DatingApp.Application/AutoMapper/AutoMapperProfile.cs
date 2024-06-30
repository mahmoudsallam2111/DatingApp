using AutoMapper;
using DatingApp.Application.Dtos;
using DatingApp.Domain.Aggregates.AppUser.Entities;

namespace DatingApp.Application.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, GetUserDto>()
                .ForMember(des => des.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain)!.Url)).ReverseMap();

            CreateMap<UserPhoto, UserPhotoDto>().ReverseMap();
            CreateMap<UserUpdateDto , AppUser>().ReverseMap();
        }
    }
}
