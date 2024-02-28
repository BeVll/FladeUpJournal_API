using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Models.Class;
using FladeUp_API.Models.Nationality;
using FladeUp_API.Models.Sex;
using System.ComponentModel.DataAnnotations;

namespace FladeUp_API.Models.User
{
    public class StudentDetailModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Image { get; set; }
        public string? IndetificateCode { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public GenderModel Gender { get; set; }
        public NationalityModel Nationality { get; set; }
        public string? Passport { get; set; }
        public bool IsLightTheme { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string Status { get; set; }
        public string? BankAccount { get; set; }

        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }

        public string? MailCountry { get; set; }
        public string? MailCity { get; set; }
        public string? MailStreet { get; set; }
        public string? MailPostalCode { get; set; }

        public string? WorkExp { get; set; }
        public List<ClassModel> Groups { get; set; }
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
    }
}
