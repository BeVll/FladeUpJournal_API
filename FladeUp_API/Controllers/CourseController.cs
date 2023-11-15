using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_Api.Data;
using FladeUp_Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FladeUp_Api.Constants;
using FladeUp_Api.Requests;
using Google.Apis.Auth;
using FladeUp_API.Models;
using FladeUp_API.Requests.Departament;
using FladeUp_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using FladeUp_API.Models.Specialization;
using FladeUp_API.Models.Corse;
using FladeUp_API.Requests.Course;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FladeUp_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        // GET: api/<CourseController>
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public CourseController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
            _appEFContext = appEFContext;
            _cloudStorage = cloudStorage;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var course = _mapper.Map<CourseModel>(_appEFContext.Courses
                    .Include(s => s.Specialization)
                    .Include(s => s.Specialization.Department)
                    .Include(s => s.Specialization.Department.Dean)
                    .Where(u => u.Id == id)
                    .SingleOrDefault());

                return Ok(course);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var courses = _appEFContext.Courses
                    .Include(s => s.Specialization)
                    .Include(s => s.Specialization.Department)
                    .Include(s => s.Specialization.Department.Dean)
                    .Select(s => _mapper.Map<CourseModel>(s))
                    .ToList();

                return Ok(courses);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CourseCreateRequest model)
        {
            try
            {
                var course = new CourseEntity()
                {
                    Name = model.Name,
                    ShortName = model.ShortName,
                    TypeOfCourse = model.TypeOfCourse,
                    SpecilizationId = model.SpecilizationId,
                    DateOfStart = model.DateOfStart,
                    DateOfEnd = model.DateOfEnd,

                };
                _appEFContext.Add(course);
                await _appEFContext.SaveChangesAsync();



                var result = _mapper.Map<CourseModel>(course);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
    }
}
