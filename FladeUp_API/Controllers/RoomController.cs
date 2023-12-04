using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
using FladeUp_Api.Data;
using FladeUp_Api.Interfaces;
using FladeUp_API.Models.Class;
using FladeUp_API.Models.Event;
using FladeUp_API.Models.User;
using FladeUp_API.Requests.Event;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FladeUp_API.Requests.Room;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FladeUp_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public RoomController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
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
                var room = _appEFContext.Rooms
                    .Where(u => u.Id == id)
                    .SingleOrDefault();

                if (room == null)
                    return NotFound();

                
                return Ok(room);

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
                var rooms = _appEFContext.Rooms
                    .ToList();

                if (rooms == null)
                    return NotFound();


                return Ok(rooms);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] RoomCreateRequest model)
        {
            try
            {
                RoomEntity room = new RoomEntity()
                {
                    Name = model.Name,
                    Description = model.Description,
                };

                _appEFContext.Add(room);
                await _appEFContext.SaveChangesAsync();


                return Ok(room);

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

                if(room == null)
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
