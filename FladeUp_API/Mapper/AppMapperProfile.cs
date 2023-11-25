﻿using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
using FladeUp_API.Models;
using FladeUp_API.Models.Class;
using FladeUp_API.Models.User;

namespace FladeUp_Api.Mapper
{
    public class AppMapperProfile : Profile
    {
        public AppMapperProfile()
        {
            CreateMap<UserEntity, UserModel>();
            CreateMap<UserEntity, UserPublicDataModel>();
            CreateMap<ClassEntity, ClassModel>();
            
        }
       
    }
}
