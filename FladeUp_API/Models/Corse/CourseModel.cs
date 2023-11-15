using FladeUp_API.Data.Entities;
using FladeUp_API.Models.Specialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Models.Corse
{
    public class CourseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public SpecializationModel Specialization { get; set; }

        public string TypeOfCourse { get; set; }

        public DateOnly DateOfStart { get; set; }
        public DateOnly DateOfEnd { get; set; }

    }
}
