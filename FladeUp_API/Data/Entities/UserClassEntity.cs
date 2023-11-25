using FladeUp_Api.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Data.Entities
{
    public class UserClassEntity
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual UserEntity User { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }
        public virtual ClassEntity Class { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
