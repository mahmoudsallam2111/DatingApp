using AutoMapper;
using DatingApp.Application.Dtos;
using DatingApp.Domain.Entities;

namespace DatingApp.Application.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, GetUserDto>();
        }
    }
}
