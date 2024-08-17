using AutoMapper;
using DatingApp.Application.Dtos;
using DatingApp.Application.Helpers;
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
            CreateMap<RegisterUserDto , AppUser>().ReverseMap();

            CreateMap<AppUser , LikeDto>().ReverseMap();


            // for custom mapper
            CreateMap(typeof(PagesList<>), typeof(PagesList<>)).ConvertUsing(typeof(PagesListConverter<,>));

            // mapper for message
            CreateMap<Message, MessageDto>()
                .ForMember(m => m.SenderPhotoUrl, u => u.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.ReceiverPhotoUrl, u => u.MapFrom(u => u.Receiver.Photos.FirstOrDefault(p => p.IsMain).Url));

            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d  => d.HasValue ? DateTime.SpecifyKind(d.Value , DateTimeKind.Utc) : null);
        }
    }
}
