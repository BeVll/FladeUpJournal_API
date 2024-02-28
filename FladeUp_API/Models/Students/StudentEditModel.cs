using FladeUp_API.Constants;
using FladeUp_API.Models.Nationality;
using FladeUp_API.Models.Sex;

namespace FladeUp_API.Models.Students
{
    public class StudentEditModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Image { get; set; }
        public string? IndetificateCode { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public GenderModel Gender { get; set; }
        public NationalityModel Nationality { get; set; }
        public string? Passport { get; set; }
        public string Status { get; set; }
        public string? BankAccount { get; set; }
        public string? WorkExp { get; set; }
        public List<GenderModel> Genders { get; set; }
        public List<NationalityModel> Nationalities { get; set; }
    }
}
