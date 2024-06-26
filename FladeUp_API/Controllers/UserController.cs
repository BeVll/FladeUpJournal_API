﻿using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_Api.Data;
using FladeUp_Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FladeUp_Api.Constants;
using FladeUp_Api.Requests;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Google.Apis.Requests.BatchRequest;
using FladeUp_API.Models.User;
using FladeUp_API.Models.Subject;
using FladeUp_API.Models.Class;

namespace FladeUp_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public UserController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
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
        [HttpGet("allUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = _appEFContext.Users.ToList();
                List<UserModel> resultUsers = new List<UserModel>();

                foreach (var item in users)
                {
                    resultUsers.Add(_mapper.Map<UserModel>(item));
                }

                return Ok(resultUsers);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("userProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value;
                var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == email);

              

                if (user != null)
                {
                    var userModel = _mapper.Map<StudentDetailModel>(user);

                    var adresses = await _appEFContext.UserAdresses.Where(a => a.UserId == user.Id).SingleOrDefaultAsync();

                    if(adresses != null)
                    {
                        userModel.Country = adresses.Country;
                        userModel.City = adresses.City;
                        userModel.Street = adresses.Street;
                        userModel.PostalCode = adresses.PostalCode;

                        userModel.MailCountry = adresses.MailCountry;
                        userModel.MailCity = adresses.MailCity;
                        userModel.MailStreet = adresses.MailStreet;
                        userModel.MailPostalCode = adresses.MailPostalCode;
                    }

                    

                    return Ok(userModel);
                }
                   

                else
                    return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getClasses")]
        public async Task<IActionResult> GetClasses()
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value;
                var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == email);



                if (user != null)
                {
                    var classes = await _appEFContext.UserClasses
                        .Where(c => c.UserId == user.Id)
                        .Include(c => c.Class)
                        .Select(c => _mapper.Map<ClassModel>(c.Class))
                        .ToListAsync();

                    foreach (var classEntity in classes)
                    {

                        var subjects = await _appEFContext.ClassSubjects
                       .Where(s => s.ClassId == classEntity.Id)
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


                        classEntity.Subjects = result;
                    }

                    return Ok(classes);
                }


                else
                    return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetAuthenticatedUser()
        {
            try
            {
                var email = User.FindFirst("Id").Value;
                var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == Convert.ToInt32(email));

                var res = _mapper.Map<StudentDetailModel>(_appEFContext.Users.Where(u => u.Id == user.Id).SingleOrDefault());
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("changeAvatar")]
        public async Task<IActionResult> ChangeAvatar([FromForm] ChangeImageRequest model)
        {
            try
            {
                    var email = User.FindFirst(ClaimTypes.Email).Value;
                var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == email);

                if (user != null)
                {
                    var fileExp = Path.GetExtension(model.Image.FileName);
                    var dirSave = Path.Combine("images/user_avatars/");
                    var rndName = Path.GetRandomFileName();
                    var imageName = dirSave + rndName + fileExp;

                    user.Image = imageName;
                    await _userManager.UpdateAsync(user);
                    await _cloudStorage.UploadFileAsync(model.Image, imageName);

                    return Ok(imageName);
                }
              
                return BadRequest("User not found!");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize]
        //[HttpPost("changeHeader")]
        //public async Task<IActionResult> ChangeHeader([FromForm] ChangeImageRequest model)
        //{
        //    try
        //    {
        //        var email = User.FindFirst(ClaimTypes.Email).Value;
        //        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == email);

        //        if (user != null)
        //        {
        //            var fileExp = Path.GetExtension(model.Image.FileName);
        //            var dirSave = Path.Combine("images/user_headers/");
        //            var rndName = Path.GetRandomFileName();
        //            var imageName = dirSave + rndName + fileExp;

        //            user.Header = imageName;
        //            await _userManager.UpdateAsync(user);
        //            await _cloudStorage.UploadFileAsync(model.Image, imageName);

        //            return Ok(imageName);
        //        }

        //        return BadRequest("User not found!");

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

    }
}
