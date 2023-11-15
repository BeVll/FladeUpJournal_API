using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
using FladeUp_API.Models;
using FladeUp_API.Models.Corse;
using FladeUp_API.Models.Group;
using FladeUp_API.Models.Specialization;
using FladeUp_API.Models.User;

namespace FladeUp_Api.Mapper
{
    public class AppMapperProfile : Profile
    {
        public AppMapperProfile()
        {
            CreateMap<UserEntity, UserModel>();
            CreateMap<UserEntity, UserPublicDataModel>();
            CreateMap<DepartmentEntity, DepartmentModel>()
                .ForMember(s => s.Firstname, opt => opt.MapFrom(t => t.Dean.Firstname))
                .ForMember(s => s.Lastname, opt => opt.MapFrom(t => t.Dean.Lastname));
            CreateMap<SpecializationEntity, SpecializationModel>();
            CreateMap<CourseEntity, CourseModel>();
            CreateMap<GroupEntity, GroupModel>();
        }
       
    }
}
