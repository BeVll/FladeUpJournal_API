using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_Api.Models;

namespace FladeUp_Api.Mapper
{
    public class AppMapperProfile : Profile
    {
        public AppMapperProfile()
        {
            CreateMap<UserEntity, UserModel>();
        }
       
    }
}
