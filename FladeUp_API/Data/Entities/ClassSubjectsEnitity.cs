using FladeUp_Api.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Data.Entities
{
    public class ClassSubjectsEnitity
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }
        public virtual ClassEntity Class { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public virtual SubjectEnitity Subject { get; set; }


        [ForeignKey("Teacher")]
        public int? TeacherId { get; set; }
        public virtual UserEntity Teacher { get; set; }
    }
}
