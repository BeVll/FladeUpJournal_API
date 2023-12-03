using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
using FladeUp_API.Models.Class;
using FladeUp_API.Models.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Models.Event
{
    public class EventModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public SubjectEnitity? Subject { get; set; }

        public UserPublicDataModel? Teacher { get; set; }
        public RoomEntity? Room { get; set; }

        public bool IsCanceled { get; set; } = false;
        public bool IsOnline { get; set; } = false;

        public List<ClassModel>? Classes { get; set; }
    }
}
