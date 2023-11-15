using FladeUp_API.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Requests.Course
{
    public class CourseCreateRequest
    {
        public string Name { get; set; }
        public string ShortName { get; set; }

        public int SpecilizationId { get; set; }

        public string TypeOfCourse { get; set; }

        public DateOnly DateOfStart { get; set; }
        public DateOnly DateOfEnd { get; set; }
    }
}
