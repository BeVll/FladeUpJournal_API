using FladeUp_Api.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Data.Entities
{
    public class GroupEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FormOfStudy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
