using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_Api.Data;
using FladeUp_Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FladeUp_Api.Constants;
using FladeUp_Api.Requests;
using Google.Apis.Auth;
using FladeUp_Api.Models;
using FladeUp_API.Models;
using FladeUp_API.Requests.Departament;
using FladeUp_API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FladeUp_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartamentController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public DepartamentController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
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
                var departament = _mapper.Map<DepartmentModel>(_appEFContext.Departaments.Include(s => s.Dean).Where(u => u.Id == id).SingleOrDefault());
                return Ok(departament);

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
                var departaments = _mapper.Map<DepartmentModel>(_appEFContext.Departaments.ToList());
                return Ok(departaments);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] DepartmentCreateRequest model)
        {
            try
            {
                var departament = new DepartmentEntity()
                {
                    Name = model.Name,
                    DeanId = model.DeanId,
                    
                };
                _appEFContext.Add(departament);
                await _appEFContext.SaveChangesAsync();



                var result = _mapper.Map<DepartmentModel>(departament);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromForm] DepartmentUpdateRequest model, int id)
        {
            try
            {
                var departament = await _appEFContext.Departaments.Include(d => d.Dean).Where(d => d.Id == id).SingleOrDefaultAsync();

                if(departament == null)
                    return BadRequest(NotFound());

                departament.Name = model.Name;
                departament.DeanId = model.DeanId;
                departament.UpdatedAt = DateTime.UtcNow;

                _appEFContext.Update(departament);
                await _appEFContext.SaveChangesAsync();

                var result = _mapper.Map<DepartmentModel>(departament);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
