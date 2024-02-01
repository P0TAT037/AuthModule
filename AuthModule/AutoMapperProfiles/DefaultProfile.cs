using AuthModule.Data.Models;
using AuthModule.DTOs;
using AutoMapper;

namespace AuthModule.AutoMapperProfiles
{
    public class DefaultProfile<TUser, TUserDto> : Profile
    {
        public DefaultProfile() 
        {
            CreateMap<TUser, TUserDto>();
            CreateMap<TUserDto, TUser>();
        }
    }
}
