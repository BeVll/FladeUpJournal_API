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

namespace FladeUp_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public SpecializationController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
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
                var specialization = _mapper.Map<SpecializationModel>(_appEFContext.Specializations
                    .Include(s => s.Department)
                    .Include(s => s.Department.Dean)
                    .Where(u => u.Id == id)
                    .SingleOrDefault());
              
                return Ok(specialization);

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
                var specializations = _appEFContext.Specializations
                    .Include(s => s.Department)
                    .Include(s => s.Department.Dean)
                    .Select(s => _mapper.Map<SpecializationModel>(s))
                    .ToList();

                return Ok(specializations);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] SpecializationCreateRequest model)
        {
            try
            {
                var specialization = new SpecializationEntity()
                {
                    Name = model.Name,
                    DepartmentId = model.DepartmentId,
                    
                };
                _appEFContext.Add(specialization);
                await _appEFContext.SaveChangesAsync();



                var result = _mapper.Map<SpecializationModel>(specialization);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromForm] SpecializationUpdateRequest model, int id)
        {
            try
            {
                var specialization = await _appEFContext.Specializations
                    .Include(d => d.Department)
                    .Include(d => d.Department.Dean)
                    .Where(d => d.Id == id)
                    .SingleOrDefaultAsync();

                if(specialization == null)
                    return BadRequest(NotFound());

                specialization.Name = model.Name;
                specialization.DepartmentId = model.DepartamentId;
                specialization.UpdatedAt = DateTime.UtcNow;

                _appEFContext.Update(specialization);
                await _appEFContext.SaveChangesAsync();

                var result = _mapper.Map<SpecializationModel>(specialization);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
