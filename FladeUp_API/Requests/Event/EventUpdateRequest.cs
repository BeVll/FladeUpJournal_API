using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Requests.Event
{
    public class EventUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public int? SubjectId { get; set; }
        public int? TeacherId { get; set; }
        public int? RoomId { get; set; }

        public bool IsCanceled { get; set; } = false;
        public bool IsOnline { get; set; } = false;

        public List<int>? ClassIds { get; set; }

    }
}
