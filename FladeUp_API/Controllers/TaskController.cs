using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
using FladeUp_Api.Data;
using FladeUp_Api.Interfaces;
using FladeUp_API.Requests.Room;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FladeUp_API.Requests.Task;
using Microsoft.EntityFrameworkCore;

namespace FladeUp_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public TaskController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
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
                var task = _appEFContext.Tasks
                    .Where(u => u.Id == id)
                    .SingleOrDefault();

                if (task == null)
                    return NotFound();


                return Ok(task);

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
                var tasks = _appEFContext.Tasks
                    .ToList();

                if (tasks == null)
                    return NotFound();


                return Ok(tasks);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] TaskCreateRequest model)
        {
            try
            {
                TaskEntity task = new TaskEntity()
                {
                    Name = model.Name,
                    Description = model.Description,
                    TypeId = model.TypeId,
                    SubjectId = model.SubjectId,
                    ClassId = model.ClassId,
                    MaximumGrade = model.MaximumGrade,
                    TeacherId = model.TeacherId,
                    DeadLine = model.DeadLine
                };

                _appEFContext.Add(task);
                await _appEFContext.SaveChangesAsync();


                return Ok(task);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] RoomUpdateRequest model)
        {
            try
            {
                var room = await _appEFContext.Rooms
                    .Where(r => r.Id == model.Id)
                    .SingleOrDefaultAsync();

                if (room == null)
                    return NotFound();

                room.Name = model.Name;
                room.Description = model.Description;

                _appEFContext.Update(room);
                await _appEFContext.SaveChangesAsync();


                return Ok(room);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var room = await _appEFContext.Rooms
                    .Where(r => r.Id == id)
                    .SingleOrDefaultAsync();

                if (room == null)
                    return NotFound();

                _appEFContext.Remove(room);
                await _appEFContext.SaveChangesAsync();


                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
