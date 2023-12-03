using FladeUp_Api.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Data.Entities
{
    public class EventEnitity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDateTime { get; set;}
        public DateTime EndDateTime { get; set; }

        [ForeignKey("Subject")]
        public int? SubjectId { get; set; }
        public virtual SubjectEnitity? Subject { get; set; }

        [ForeignKey("Teacher")]
        public int? TeacherId { get; set; }
        public virtual UserEntity? Teacher { get; set; }

        [ForeignKey("Room")]
        public int? RoomId { get; set; }
        public virtual RoomEntity? Room {  get; set; }

        public bool IsCanceled { get; set; } = false;
        public bool IsOnline { get; set; } = false;

    }
}
