using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
using FladeUp_Api.Data;
using FladeUp_Api.Interfaces;
using FladeUp_API.Requests.Subject;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FladeUp_API.Models.Class;
using FladeUp_API.Models.User;
using FladeUp_API.Models.Event;
using FladeUp_API.Requests.Class;
using FladeUp_API.Requests.Event;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FladeUp_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        // GET: api/<EventController>
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public EventController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
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
                var eventEntity = _appEFContext.Events
                    .Where(u => u.Id == id)
                    .Include(u => u.Teacher)
                    .Include(u => u.Subject)
                    .Include(u => u.Room)
                    .SingleOrDefault();

                if(eventEntity == null) 
                    return NotFound();

                var eventModel = _mapper.Map<EventModel>(eventEntity);

                if(eventEntity.Teacher != null)
                    eventModel.Teacher = _mapper.Map<UserPublicDataModel>(eventEntity.Teacher);


                var classes = await _appEFContext.EventClasses
                    .Where(e => e.EventId == eventEntity.Id)
                    .Include(e => e.Class)
                    .Select(e => _mapper.Map<ClassModel>(e.Class))    
                    .ToListAsync();

                eventModel.Classes = classes;

                return Ok(eventModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("forUserByDate")]
        public async Task<IActionResult> GetEventsOfDateForUser([FromQuery] int userId, [FromQuery] string date)
        {
            try
            {
                var dt = DateTime.Parse(date);


                var userClasses = await _appEFContext.UserClasses
                    .Where(u => u.UserId == userId)
                    .ToListAsync();

                var eventEntity = await _appEFContext.EventClasses
                    .Include(e => e.Event)
                    .Where(u => u.Event.StartDateTime.Year == dt.Year && u.Event.StartDateTime.Month == dt.Month && u.Event.StartDateTime.Day == dt.Day)
                    .Include(u => u.Event.Subject)
                    .Include(u => u.Event.Teacher)
                    .Include(u => u.Event.Room)
                    .ToListAsync();



                List<EventModel> events = new List<EventModel>();

                foreach (var item in eventEntity)
                {
                    if(userClasses.Find(s => s.ClassId == item.ClassId) != null)
                    {
                        var eventModel = _mapper.Map<EventModel>(item.Event);

                        if (item.Event.Teacher != null)
                            eventModel.Teacher = _mapper.Map<UserPublicDataModel>(item.Event.Teacher);

                        var classes = await _appEFContext.EventClasses
                       .Where(e => e.EventId == item.EventId)
                       .Include(e => e.Class)
                       .Select(e => _mapper.Map<ClassModel>(e.Class))
                       .ToListAsync();

                        eventModel.Classes = classes;

                        events.Add(eventModel);
                    }
                    
                }

                var result = events.OrderBy(e => e.StartDateTime);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] EventCreateRequest model)
        {
            try
            {
                var eventEntity = new EventEnitity()
                {
                    Name = model.Name,
                    Description = model.Description,
                    StartDateTime = model.StartDateTime,
                    EndDateTime = model.EndDateTime,
                    TeacherId = model.TeacherId,
                    SubjectId = model.SubjectId,
                    RoomId = model.RoomId,
                    IsCanceled = model.IsCanceled,
                    IsOnline = model.IsOnline,
                };

                _appEFContext.Add(eventEntity);
                await _appEFContext.SaveChangesAsync();

                if (model.ClassIds.Count() > 0 )
                {
                    foreach (var classId in model.ClassIds)
                    {
                        EventClassesEntity eventClass = new EventClassesEntity()
                        {
                            ClassId = classId,
                            EventId = eventEntity.Id,
                        };

                        _appEFContext.Add(eventClass);
                    }
                    
                }
                await _appEFContext.SaveChangesAsync();


                var eventModel = _mapper.Map<EventModel>(eventEntity);

                if (eventEntity.Teacher != null)
                    eventModel.Teacher = _mapper.Map<UserPublicDataModel>(eventEntity.Teacher);

                return Ok(eventModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] EventUpdateRequest model)
        {
            try
            {
                var eventEntity = await _appEFContext.Events
                    .Where(e => e.Id == model.Id)
                    .SingleOrDefaultAsync();

                if (eventEntity == null)
                    return NotFound();


                eventEntity.Name = model.Name;
                eventEntity.Description = model.Description;
                eventEntity.StartDateTime = model.StartDateTime;
                eventEntity.EndDateTime = model.EndDateTime;
                eventEntity.TeacherId = model.TeacherId;
                eventEntity.SubjectId = model.SubjectId;
                eventEntity.RoomId = model.RoomId;
                eventEntity.IsCanceled = model.IsCanceled;
                eventEntity.IsOnline = model.IsOnline;
            

                _appEFContext.Update(eventEntity);
                await _appEFContext.SaveChangesAsync();

                var classes = _appEFContext.EventClasses.Where(e => e.EventId == eventEntity.Id).ToList();
                foreach (var item in classes)
                {
                    _appEFContext.Remove(item); 
                }

                if (model.ClassIds.Count() > 0)
                {
                    
                    foreach (var classId in model.ClassIds)
                    {
                        
                        EventClassesEntity eventClass = new EventClassesEntity()
                        {
                            ClassId = classId,
                            EventId = eventEntity.Id,
                        };

                        _appEFContext.Add(eventClass);
                    }

                }
                await _appEFContext.SaveChangesAsync();


                var eventModel = _mapper.Map<EventModel>(eventEntity);

                if (eventEntity.Teacher != null)
                    eventModel.Teacher = _mapper.Map<UserPublicDataModel>(eventEntity.Teacher);

                return Ok(eventModel);

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
                var eventEntity = await _appEFContext.Events
                    .Where(r => r.Id == id)
                    .SingleOrDefaultAsync();

                if (eventEntity == null)
                    return NotFound();

                var eventClasses = await _appEFContext.EventClasses
                    .Where(e => e.EventId == eventEntity.Id)
                    .ToListAsync();

                foreach (var eventClass in eventClasses)
                {
                    _appEFContext.Remove(eventClass);
                }

                _appEFContext.Remove(eventEntity);
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
