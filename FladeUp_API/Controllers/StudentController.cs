using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_Api.Data;
using FladeUp_Api.Interfaces;
using FladeUp_API.Models.Class;
using FladeUp_API.Models.Subject;
using FladeUp_API.Models.User;
using FladeUp_Api.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FladeUp_API.Models.Students;
using FladeUp_Api.Constants;
using Microsoft.EntityFrameworkCore;
using FladeUp_API.Data.Entities;
using FladeUp_API.Models;
using System.Linq.Expressions;
using FladeUp_Api.Services;
using FladeUp_API.Requests.Class;
using FladeUp_API.Requests.Student;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FladeUp_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public StudentController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
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
                var student = _mapper.Map<StudentDetailModel>(
                    _appEFContext.Users.Where(
                        u => u.Id == id && u.UserRoles.Where(r => r.Role.Name == "Student").FirstOrDefault() != null)
                    .SingleOrDefault());

                student.Groups = await _appEFContext.UserClasses
                    .Where(g => g.UserId == id)
                    .Select(g => _mapper.Map<ClassModel>(g.Class))
                    .ToListAsync();
                return Ok(student);

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
                var users = _appEFContext.Users.Where(
                         u => u.UserRoles.Where(r => r.Role.Name == "Student").FirstOrDefault() != null)
                    .ToList();
                List<StudentModel> resultUsers = new List<StudentModel>();

                foreach (var item in users)
                {
                    resultUsers.Add(_mapper.Map<StudentModel>(item));
                }

                return Ok(resultUsers);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("addresses/{id}")]
        public async Task<IActionResult> GetAdressesForStudent(int id)
        {
            try
            {
                var userAdresses = await _appEFContext.UserAdresses.Where(r => r.UserId == id).FirstOrDefaultAsync();

                if (userAdresses == null)
                    return NotFound();

                return Ok(userAdresses);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetStudents([FromQuery] string? searchQuery, [FromQuery] string? sortBy, [FromQuery] string? sortDirection, [FromQuery] int page, [FromQuery] int pageSize)
        {
            try
            {
                var studentsQuery = _appEFContext.Users
                    .Include(s => s.UserRoles)
                    .Where(u => u.UserRoles.Any(r => r.Role.Name == "Student"))
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var lowerSearchQuery = searchQuery.ToLower();

                    studentsQuery = studentsQuery
                        .Where(s => s.Firstname.ToLower().Contains(lowerSearchQuery) ||
                                    s.Lastname.ToLower().Contains(lowerSearchQuery) ||
                                    $"{s.Firstname.ToLower()} {s.Lastname.ToLower()}".Contains(lowerSearchQuery) ||
                                    s.Id.ToString() == lowerSearchQuery);
                }

                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "firstname":
                            studentsQuery = ApplySort(studentsQuery, s => s.Firstname, sortDirection);
                            break;
                        case "lastname":
                            studentsQuery = ApplySort(studentsQuery, s => s.Lastname, sortDirection);
                            break;
                        case "id":
                            studentsQuery = ApplySort(studentsQuery, s => s.Id, sortDirection);
                            break;
                        // Add additional cases for other fields if needed
                        default:
                            // Default sorting behavior if sortBy is not recognized
                            studentsQuery = ApplySort(studentsQuery, s => s.Id, sortDirection);
                            break;
                    }
                }

                var totalRecords = await studentsQuery.CountAsync();

                var students = await studentsQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(s => _mapper.Map<StudentModel>(s))
                    .ToListAsync();

                var totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalRecords) / Convert.ToDecimal(pageSize)));

                return Ok(new PagedResponse<List<StudentModel>>(students, page, pageSize, totalPages, totalRecords));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private IQueryable<T> ApplySort<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> keySelector, string? sortDirection)
        {
            if (string.IsNullOrEmpty(sortDirection) || sortDirection.ToLower() == "ascending")
                return query.OrderBy(keySelector);
            else
                return query.OrderByDescending(keySelector);
        }

        
      

        [HttpPost("create")]
        public async Task<IActionResult> Signup([FromForm] RegisterRequest model)
        {
            try
            {

                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = "";

                if (user == null)
                {
                    var client = new HttpClient();
                    string fileName = null;
                    string extension = ".png";

                    var newUser = new UserEntity()
                    {
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        Email = model.Email,
                        UserName = model.Email,
                        Image = null,
                        IndetificateCode = model.IndetificateCode,
                        PlaceOfBirth = model.PlaceOfBirth,
                        Sex = model.Sex,
                        National = model.National,
                        Passport = model.Passport,
                        IsLightTheme = false,
                        Instagram = null,
                        Facebook = null,
                        Twitter = null,
                        Status = "Student",
                        CreatedAt = DateTime.UtcNow,
                        DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow),
                    };
                    var fileExp = Path.GetExtension(model.Image.FileName);
                    var dirSave = Path.Combine("images/user_avatars/");
                    var rndName = Path.GetRandomFileName();
                    var imageName = dirSave + rndName + fileExp;

                    newUser.Image = imageName;

                    await _cloudStorage.UploadFileAsync(model.Image, imageName);

                    var result = await _userManager.CreateAsync(newUser);



                    if (result.Succeeded)
                    {
                        var id = await _userManager.GetUserIdAsync(newUser);
                        await _userManager.AddPasswordAsync(newUser, model.Password);
                        await _userManager.UpdateAsync(newUser);

                        result = await _userManager.AddToRoleAsync(newUser, Roles.Student);
                        token = await _jwtTokenService.CreateToken(newUser);

                        var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        var user2 = await _appEFContext.Users.Where(u => u.Email == newUser.Email).SingleOrDefaultAsync();
                        await _userManager.ConfirmEmailAsync(user2, confirmToken);
                        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(confirmToken);
                        var confirmUrl = System.Convert.ToBase64String(plainTextBytes);

                        //var body = @"
                        //                <html lang=""en"">
                        //                    <body style=""padding: 0; margin: 0; font-family: Inter, system-ui, Avenir, Helvetica, Arial, sans-serif;"">
                        //                      <div style=""background-color: #121212;justify-content: center; align-items: center; padding: 50px 10%; color: #fff;"">
                        //                        <div style="" width: 100%; background-color: #1E1E1E; align-items: start; justify-content: start; padding: 25px; border-radius: 10px;"">
                        //                          <div style=""font-size: 24px; color: #f2f0f0; font-weight: bold;"">
                        //                            Please confirm your email
                        //                          </div>
                        //                          <div style=""color: #f2f0f0; font-size: 20px;"">
                        //                            Welcome " + newUser.Firstname + @" to FladeUp!
                        //                          </div>
                        //                          <div class=""buttonBlock"" style=""width: 100%; display: flex; justify-content: start; margin-top: 25px;"">
                        //                            <a class=""btn"" href=""http://localhost:5173/confirmEmail/" + user2.Id + "?token=" + confirmUrl + @""" style=""border-radius: 10px; padding: 8px 20px; transition: 0.2s; border: 1px solid #2EBF91; background-color: #353535; color: #f2f0f0; font-weight: bold; cursor: pointer;"">Click to confirm</a>
                        //                          </div>
                        //                        </div>
                        //                      </div>
                        //                    </body>

                        //                </html>";
                        //_emailService.SendMailAsync("FladeUp | Confirmation link", body, newUser.Email);
                        return Ok(confirmToken);
                    }
                }

                //http://localhost:5173/confirmEmail?token="+ confirmToken.Result.ToString() + 

                return BadRequest("Email is registered");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addToGroups")]
        public async Task<IActionResult> AddToGroups([FromForm] AddToGroupsRequest model)
        {
            try
            {
                foreach (var item in model.GroupIds)
                {
                    var checkExist = await _appEFContext.UserClasses.Where(g => g.ClassId == item && g.UserId == model.StudentId).ToListAsync();

                    if (checkExist.Count > 0)
                        return BadRequest("User exist!");

                    var groupAddStudent = new UserClassEntity()
                    {
                        UserId = model.StudentId,
                        ClassId = item,

                    };
                    _appEFContext.Add(groupAddStudent);
                    
                }
                await _appEFContext.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updateAdresses/{id}")]
        public async Task<IActionResult> UpdateAdresses(int id, [FromForm] UpdateAdresses model)
        {
            try
            {
                var userAdresses = await _appEFContext.UserAdresses.Where(u => u.UserId == id).FirstOrDefaultAsync();

                if (userAdresses == null)
                    return NotFound();

                userAdresses.Country = model.Country;
                userAdresses.City = model.City;
                userAdresses.Street = model.Street;
                userAdresses.PostalCode = model.PostalCode;
                userAdresses.MailCountry = model.MailCountry;
                userAdresses.MailCity = model.MailCity;
                userAdresses.MailStreet = model.MailStreet;
                userAdresses.MailPostalCode = model.MailPostalCode;
                userAdresses.UpdatedAt = DateTime.UtcNow;

                _appEFContext.Update(userAdresses);
                await _appEFContext.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("removeFromGroup")]
        public async Task<IActionResult> RemoveFromGroup([FromQuery] int studentId, [FromQuery] int groupId)
        {
            try
            {
                var studentGroup = await _appEFContext.UserClasses.Where(g => g.ClassId == groupId && g.UserId == studentId).FirstOrDefaultAsync();

                if (studentGroup == null)
                    return BadRequest("User doesn`t exist in group!");

                    
                _appEFContext.Remove(studentGroup);
                await _appEFContext.SaveChangesAsync();
                return Ok();

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
                var student = await _appEFContext.Users
                    .Include(s => s.UserRoles)
                    .Where(u => u.Id == id && u.UserRoles.Any(r => r.Role.Name == "Student"))
                    .SingleOrDefaultAsync();

                if (student == null)
                    return NotFound();

                _appEFContext.Remove(student);
                await _appEFContext.SaveChangesAsync();


                return Ok();
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
