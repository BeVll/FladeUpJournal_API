using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Requests.Class
{
    public class ClassAddSubjectRequest
    {
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int? TeacherId { get; set; }
        public string? Description { get; set; }
    }
}
