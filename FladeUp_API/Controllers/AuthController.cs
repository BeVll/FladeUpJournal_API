using AutoMapper;
using FladeUp_Api.Constants;
using FladeUp_Api.Data;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_Api.Interfaces;
using FladeUp_Api.Requests;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FladeUp_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;
        private readonly IEmailService _emailService;
        public AuthController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage, IEmailService emailService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
            _appEFContext = appEFContext;
            _cloudStorage = cloudStorage;
            _emailService = emailService;
        }


        [HttpPost("signup")]
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

                    var result = await _userManager.CreateAsync(newUser);
                        


                    if (result.Succeeded)
                    {
                            var id = await _userManager.GetUserIdAsync(newUser);
                            await _userManager.AddPasswordAsync(newUser, model.Password);
                            await _userManager.UpdateAsync(newUser);
                            
                            result = await _userManager.AddToRoleAsync(newUser, Roles.Student);
                            token = await _jwtTokenService.CreateToken(newUser);
                       
                        var confirmToken = _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        var user2 = await _appEFContext.Users.Where(u => u.Email == newUser.Email).SingleOrDefaultAsync();
                        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(confirmToken.Result.ToString());
                        var confirmUrl = System.Convert.ToBase64String(plainTextBytes);

                        var body = @"
                                        <html lang=""en"">
                                            <body style=""padding: 0; margin: 0; font-family: Inter, system-ui, Avenir, Helvetica, Arial, sans-serif;"">
                                              <div style=""background-color: #121212;justify-content: center; align-items: center; padding: 50px 10%; color: #fff;"">
                                                <div style="" width: 100%; background-color: #1E1E1E; align-items: start; justify-content: start; padding: 25px; border-radius: 10px;"">
                                                  <div style=""font-size: 24px; color: #f2f0f0; font-weight: bold;"">
                                                    Please confirm your email
                                                  </div>
                                                  <div style=""color: #f2f0f0; font-size: 20px;"">
                                                    Welcome "+newUser.Firstname+ @" to FladeUp!
                                                  </div>
                                                  <div class=""buttonBlock"" style=""width: 100%; display: flex; justify-content: start; margin-top: 25px;"">
                                                    <a class=""btn"" href=""http://localhost:5173/confirmEmail/"+ user2.Id + "?token=" + confirmUrl + @""" style=""border-radius: 10px; padding: 8px 20px; transition: 0.2s; border: 1px solid #2EBF91; background-color: #353535; color: #f2f0f0; font-weight: bold; cursor: pointer;"">Click to confirm</a>
                                                  </div>
                                                </div>
                                              </div>
                                            </body>

                                        </html>";
                            _emailService.SendMailAsync("FladeUp | Confirmation link", body, newUser.Email);
                        return Ok(confirmToken.Result);
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


        //[HttpPost("loginGoogle")]
        //public async Task<IActionResult> LoginGoogle([FromForm] GoogleLoginRequest model)
        //{
        //    try
        //    {
        //        Console.Write("sdfsd");

        //        GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
        //        GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(model.Token, settings).Result;
                
        //        if (payload == null)
        //            return BadRequest("Login error!");

        //        var user = await _userManager.FindByEmailAsync(payload.Email);
        //        var token = "";

        //        if (user == null)
        //        {
        //            var client = new HttpClient();
        //            string fileName = null;
        //            string extension = ".png";

        //            using (var response = await client.GetAsync(payload.Picture))
        //            {

        //                var fileExp = response.Content.Headers.ContentType.MediaType.Replace("image/", ".");
        //                var dirSave = Path.Combine("images/user_avatars/");
        //                var rndName = Path.GetRandomFileName();
        //                var imageName = dirSave + rndName + fileExp;
        //                Stream stream = response.Content.ReadAsStream();
        //                _cloudStorage.UploadFileAsync(new FormFile(stream, 0, stream.Length, null, rndName), imageName);

        //                var newUser = new UserEntity()
        //                {
        //                    Name = payload.Name,
        //                    Email = payload.Email,
        //                    Image = imageName,
        //                    UserName = "user" + _userManager.Users.Count() + 1.ToString(),
        //                    Description = "",
        //                    Verified = false,
        //                    Country = model.Country,
        //                    CountryCode = model.CountryCode.ToLower(),
        //                    City = "",
        //                    IsLightTheme = false,
        //                    Header = null,
        //                    Instagram = null,
        //                    Facebook = null,
        //                    Twitter = null,
        //                    CreatedAt = DateTime.UtcNow,
        //                };
        //                var result = await _userManager.CreateAsync(newUser);

        //                if (result.Succeeded)
        //                {
        //                    var id = await _userManager.GetUserIdAsync(newUser);
        //                    newUser.UserName = "user" + id.ToString();
        //                    await _userManager.UpdateAsync(newUser);
        //                    result = await _userManager.AddToRoleAsync(newUser, Roles.User);
        //                    token = await _jwtTokenService.CreateToken(newUser);
        //                    return Ok(new { token });
        //                }
        //            }
        //        }
        //        if (!user.EmailConfirmed)
        //        {
        //            user.EmailConfirmed = true;
        //            await _userManager.UpdateAsync(user);
        //        }
        //        token = await _jwtTokenService.CreateToken(user);
        //        return Ok(new { token });

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = "";

                
                if (user == null)
                    return BadRequest("User doen`t exist");

                //if (!user.EmailConfirmed)
                //    return BadRequest("Email is not confirmed! To you email address was sended message with confirmation button(link)");

                var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);

                if(!passwordCheck)
                    return BadRequest("Wrong password");

                token = await _jwtTokenService.CreateToken(user);
                return Ok(new { token });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromForm] ConfirmEmailRequest model)
        {
            try
            {
                var user = await _appEFContext.Users.Where(u => u.Id == model.Id).SingleOrDefaultAsync();

                if (user == null)
                    return BadRequest("User doesn`t exist!");

                var base64EncodedBytes = System.Convert.FromBase64String(model.Token);
                var token = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                var res = await _userManager.ConfirmEmailAsync(user, token);

                if(res.Succeeded)
                {
                    return Ok();
                }

                return BadRequest(res.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
