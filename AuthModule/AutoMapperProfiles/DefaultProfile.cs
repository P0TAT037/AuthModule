using AuthModule.Data.Models;
using AuthModule.DTOs;
using AutoMapper;

namespace AuthModule.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile() 
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}
