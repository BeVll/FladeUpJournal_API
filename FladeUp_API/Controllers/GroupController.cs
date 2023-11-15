using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_Api.Data;
using FladeUp_Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FladeUp_Api.Constants;
using FladeUp_Api.Requests;
using Google.Apis.Auth;
using FladeUp_API.Requests.Departament;
using FladeUp_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using FladeUp_API.Models.Specialization;
using FladeUp_API.Models.Corse;
using FladeUp_API.Requests.Course;
using FladeUp_API.Models.Group;
using FladeUp_API.Models.User;
using FladeUp_API.Requests.Group;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FladeUp_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        // GET: api/<CourseController>
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public GroupController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
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
                var group = _mapper.Map<GroupModel>(_appEFContext.Groups
                    .Where(u => u.Id == id)
                    .SingleOrDefault());

                group.Students = await _appEFContext.UserGroups.Include(u => u.User).Where(u => u.GroupId == id).Select(u => _mapper.Map<UserPublicDataModel>(u)).ToListAsync();

                return Ok(group);

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
                var groups = await _appEFContext.Groups
                    .Select(g => _mapper.Map<GroupModel>(g))
                    .ToListAsync();

                if(groups != null)
                {
                    foreach (GroupModel item in groups)
                    {
                        item.Students = await _appEFContext.UserGroups
                            .Include(u => u.User)
                            .Where(u => u.GroupId == item.Id)
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
        public async Task<IActionResult> Create([FromForm] GroupCreateRequest model)
        {
            try
            {
                var course = new GroupEntity()
                {
                    Name = model.Name,
                    FormOfStudy = model.FormOfStudy,

                };
                _appEFContext.Add(course);
                await _appEFContext.SaveChangesAsync();



                var result = _mapper.Map<GroupModel>(course);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addStudent")]
        public async Task<IActionResult> AddStudent([FromForm] GroupAddRemoveStudentRequest model)
        {
            try
            {

                var checkExist = await _appEFContext.UserGroups.Where(g => g.GroupId == model.GroupId && g.UserId == model.UserId).ToListAsync();

                if (checkExist.Count > 0)
                    return BadRequest("User exist!");

                var groupAddStudent = new UserGroupEntity()
                {
                    UserId = model.UserId,
                    GroupId = model.GroupId,

                };
                _appEFContext.Add(groupAddStudent);
                await _appEFContext.SaveChangesAsync();



                var group = _mapper.Map<GroupModel>(_appEFContext.Groups
                    .Where(u => u.Id == groupAddStudent.GroupId)
                    .SingleOrDefault());

                group.Students = await _appEFContext.UserGroups
                            .Include(u => u.User)
                            .Where(u => u.GroupId == groupAddStudent.GroupId)
                            .Select(u => _mapper.Map<UserPublicDataModel>(u.User))
                            .ToListAsync();

                return Ok(group);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("removeStudent")]
        public async Task<IActionResult> RemoveStudent([FromForm] GroupAddRemoveStudentRequest model)
        {
            try
            {

                var checkExist = await _appEFContext.UserGroups.Where(g => g.GroupId == model.GroupId && g.UserId == model.UserId).SingleOrDefaultAsync();

                if (checkExist == null)
                    return BadRequest(NotFound("User not found in group"));

                
                _appEFContext.Remove(checkExist);
                await _appEFContext.SaveChangesAsync();



                var group = _mapper.Map<GroupModel>(_appEFContext.Groups
                    .Where(u => u.Id == model.GroupId)
                    .SingleOrDefault());

                group.Students = await _appEFContext.UserGroups
                            .Include(u => u.User)
                            .Where(u => u.GroupId == model.GroupId)
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
