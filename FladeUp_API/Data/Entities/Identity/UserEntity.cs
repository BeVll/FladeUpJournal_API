using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FladeUp_Api.Data.Entities.Identity
{
    public class UserEntity : IdentityUser<int>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Image { get; set; }
        public string? IndetificateCode { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public int GenderId { get; set; }
        public int NationalityId { get; set; }
        public string? Passport { get; set; }
        public bool IsLightTheme { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }

        public string? WorkExp { get; set; }

        public string? BankAccount { get; set; }

        public string Status { get; set; }
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
