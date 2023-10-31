using FladeUp_Api.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_Api.Data.Entities
{
    public class PostEntity
    {
        public int Id { get; set; }
        public string? PostText { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public List<int>? TagIds { get; set; }
        public virtual List<TagEnitity>? Tags { get; set; }
        public DateTime PostTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
