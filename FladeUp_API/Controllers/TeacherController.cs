using AutoMapper;
using FladeUp_Api.Constants;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
using FladeUp_Api.Data;
using FladeUp_Api.Interfaces;
using FladeUp_API.Models.Class;
using FladeUp_API.Models.Students;
using FladeUp_API.Models.Subject;
using FladeUp_API.Models.User;
using FladeUp_API.Models;
using FladeUp_API.Requests.Student;
using FladeUp_Api.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using FladeUp_API.Models.Teacher;

namespace FladeUp_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeacherController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public TeacherController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
            _appEFContext = appEFContext;
            _cloudStorage = cloudStorage;
        }

       
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTeachers()
        {
            try
            {
                var teachers = _appEFContext.Users.Where(
                         u => u.UserRoles.Where(r => r.Role.Name == "Teacher").FirstOrDefault() != null)
                    .ToList();
                List<TeacherModel> resultUsers = new List<TeacherModel>();

                foreach (var item in teachers)
                {
                    resultUsers.Add(_mapper.Map<TeacherModel>(item));
                }

                return Ok(resultUsers);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
