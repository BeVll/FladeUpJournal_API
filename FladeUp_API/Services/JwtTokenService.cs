using FladeUp_Api.Data.Entities.Identity;
using FladeUp_Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FladeUp_Api.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<UserEntity> _userManager;
        public JwtTokenService(IConfiguration configuration, UserManager<UserEntity> userManager)
        {
            _config = configuration;
            _userManager = userManager;
        }
        public async Task<string> CreateToken(UserEntity user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim("firstname", user.Firstname),
                 new Claim("lastname", user.Lastname),
                new Claim("image", user.Image ?? string.Empty),
                new Claim("email", user.Email)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<String>("JwtSecretKey")));
            var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                signingCredentials: signinCredentials,
                expires: DateTime.Now.AddDays(10),
                claims: claims
           );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
