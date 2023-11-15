using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Data.Entities
{
    public class CourseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        [ForeignKey("Specialization")]
        public int SpecilizationId { get; set; }
        public virtual SpecializationEntity Specialization { get; set; }

        public string TypeOfCourse { get; set; }

        public DateOnly DateOfStart { get; set; }
        public DateOnly DateOfEnd { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }
}
