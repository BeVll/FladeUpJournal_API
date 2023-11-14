using FladeUp_Api.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Data.Entities
{
    public class DepartmentEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Dean")]
        public int DeanId { get; set; }
        public virtual UserEntity Dean { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }
}
