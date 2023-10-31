using FladeUp_Api.Data.Entities.Identity;

namespace FladeUp_Api.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> CreateToken(UserEntity user);
    }
}
