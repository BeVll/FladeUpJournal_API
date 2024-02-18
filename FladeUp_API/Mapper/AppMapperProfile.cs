using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
using FladeUp_API.Models;
using FladeUp_API.Models.Class;
using FladeUp_API.Models.Event;
using FladeUp_API.Models.Students;
using FladeUp_API.Models.Subject;
using FladeUp_API.Models.User;

namespace FladeUp_Api.Mapper
{
    public class AppMapperProfile : Profile
    {
        public AppMapperProfile()
        {
            CreateMap<UserEntity, StudentDetailModel>();
            CreateMap<UserEntity, StudentModel>();
            CreateMap<UserEntity, UserPublicDataModel>();
            CreateMap<ClassEntity, ClassModel>();
            CreateMap<EventEnitity, EventModel>();
            CreateMap<SubjectEnitity, SubjectModel>();
        }
       
    }
}
