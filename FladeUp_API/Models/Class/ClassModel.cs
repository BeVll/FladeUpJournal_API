using FladeUp_API.Models.Subject;
using FladeUp_API.Models.User;
using Microsoft.EntityFrameworkCore;

namespace FladeUp_API.Models.Class
{
    public class ClassModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string FormOfStudy { get; set; }
        public int YearOfStart { get; set; }
        public int YearOfEnd { get; set; }
        public List<UserPublicDataModel>? Students { get; set; } = new List<UserPublicDataModel>();

        public List<SubjectModel>? Subjects { get; set; } = new List<SubjectModel>();
    }
}
