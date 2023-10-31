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

namespace FladeUp_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public PostController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
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
                var user = _mapper.Map<UserModel>(_appEFContext.Users.Where(u => u.Id == id).SingleOrDefault());
                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
