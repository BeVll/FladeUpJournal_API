using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_Api.Data;
using FladeUp_Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FladeUp_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using FladeUp_API.Models.Class;
using FladeUp_API.Models.User;
using FladeUp_API.Requests.Class;
using FladeUp_API.Models.Subject;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FladeUp_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        // GET: api/<CourseController>
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public ClassController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
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
                var classModel = _mapper.Map<ClassModel>(_appEFContext.Classes
                    .Where(u => u.Id == id)
                    .SingleOrDefault());

                classModel.Students = await _appEFContext.UserClasses
                    .Include(u => u.User)
                    .Where(u => u.ClassId == id)
                    .Select(u => _mapper.Map<UserPublicDataModel>(u))
                    .ToListAsync();

                return Ok(classModel);

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
                var groups = await _appEFContext.Classes
                    .Select(g => _mapper.Map<ClassModel>(g))
                    .ToListAsync();

                if(groups != null)
                {
                    foreach (ClassModel item in groups)
                    {
                        item.Students = await _appEFContext.UserClasses
                            .Include(u => u.User)
                            .Where(u => u.ClassId == item.Id)
                            .Select(u => _mapper.Map<UserPublicDataModel>(u.User))
                            .ToListAsync();

                    }
                }
                    

                

                return Ok(groups);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("forUser")]
        public async Task<IActionResult> GetForUser([FromQuery] int userId)
        {
            try
            {
             

                var groups = await _appEFContext.UserClasses
                    .Include(g => g.Class)
                    .Where(g => g.UserId == userId)
                    .Select(g => _mapper.Map<ClassModel>(g.Class))
                    .ToListAsync();

                if (groups != null)
                {
                    foreach (ClassModel item in groups)
                    {
                        item.Students = await _appEFContext.UserClasses
                            .Include(u => u.User)
                            .Where(u => u.ClassId == item.Id)
                            .Select(u => _mapper.Map<UserPublicDataModel>(u.User))
                            .ToListAsync();

                    }
                }




                return Ok(groups);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] ClassCreateRequest model)
        {
            try
            {
                var course = new ClassEntity()
                {
                    Name = model.Name,
                    ShortName = model.ShortName,
                    FormOfStudy = model.FormOfStudy,
                    YearOfStart = model.YearOfStart,
                    YearOfEnd = model.YearOfEnd,
                    ClassSpecialization = model.ClassSpecialization,

                };
                _appEFContext.Add(course);
                await _appEFContext.SaveChangesAsync();



                var result = _mapper.Map<ClassModel>(course);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getSubjects/{classId}")]
        public async Task<IActionResult> GetSubjects(int classId)
        {
            try
            {
                var subjects = await _appEFContext.ClassSubjects
                    .Where(s => s.ClassId ==  classId)
                    .Include(s => s.Subject)
                    .Include(s => s.Teacher)
                    .ToListAsync();

                var result = new List<SubjectModel>();

                foreach (var item in subjects)
                {
                    result.Add(new SubjectModel
                    {
                        Id = item.SubjectId,
                        Name = item.Subject.Name,
                        Teacher = _mapper.Map<UserPublicDataModel>(item.Teacher),
                        Description = item.Description,
                        Color = item.Subject.Color,
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addSubject")]
        public async Task<IActionResult> AddSubject([FromForm] ClassAddSubjectRequest model)
        {
            try
            {
                ClassSubjectsEnitity classSubjectsEnitity = new ClassSubjectsEnitity()
                {
                    ClassId = model.ClassId,
                    SubjectId = model.SubjectId,
                    TeacherId = model.TeacherId,
                    Description = model.Description,
                };

                _appEFContext.Add(classSubjectsEnitity);
                await _appEFContext.SaveChangesAsync();

                return Ok(classSubjectsEnitity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addStudent")]
        public async Task<IActionResult> AddStudent([FromForm] ClassAddRemoveStudentRequest model)
        {
            try
            {

                var checkExist = await _appEFContext.UserClasses.Where(g => g.ClassId == model.ClassId && g.UserId == model.UserId).ToListAsync();

                if (checkExist.Count > 0)
                    return BadRequest("User exist!");

                var groupAddStudent = new UserClassEntity()
                {
                    UserId = model.UserId,
                    ClassId = model.ClassId,

                };
                _appEFContext.Add(groupAddStudent);
                await _appEFContext.SaveChangesAsync();



                var classModel = _mapper.Map<ClassModel>(_appEFContext.Classes
                    .Where(u => u.Id == groupAddStudent.ClassId)
                    .SingleOrDefault());

                classModel.Students = await _appEFContext.UserClasses
                            .Include(u => u.User)
                            .Where(u => u.ClassId == groupAddStudent.ClassId)
                            .Select(u => _mapper.Map<UserPublicDataModel>(u.User))
                            .ToListAsync();

                return Ok(classModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("removeStudent")]
        public async Task<IActionResult> RemoveStudent([FromForm] ClassAddRemoveStudentRequest model)
        {
            try
            {

                var checkExist = await _appEFContext.UserClasses.Where(g => g.ClassId == model.ClassId && g.UserId == model.UserId).SingleOrDefaultAsync();

                if (checkExist == null)
                    return BadRequest(NotFound("User not found in group"));

                
                _appEFContext.Remove(checkExist);
                await _appEFContext.SaveChangesAsync();



                var group = _mapper.Map<ClassModel>(_appEFContext.Classes
                    .Where(u => u.Id == model.ClassId)
                    .SingleOrDefault());

                group.Students = await _appEFContext.UserClasses
                            .Include(u => u.User)
                            .Where(u => u.ClassId == model.ClassId)
                            .Select(u => _mapper.Map<UserPublicDataModel>(u.User))
                            .ToListAsync();

                return Ok(group);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
