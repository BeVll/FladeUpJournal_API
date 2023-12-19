using FladeUp_API.Data.Entities;
using FladeUp_API.Models.Class;
using FladeUp_API.Models.Subject;
using FladeUp_API.Models.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Models.Task
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public virtual TaskTypeEntity Type { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public virtual SubjectModel Subject { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }
        public virtual ClassModel Class { get; set; }

        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }
        public virtual UserPublicDataModel Teacher { get; set; }

        public int MaximumGrade { get; set; }
        public DateTime? DeadLine { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
    }
}
