using FladeUp_Api.Data.Entities.Identity;
using FladeUp_API.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Requests.Departament
{
    public class SpecializationCreateRequest
    {
        public string Name { get; set; }
        public int DepartmentId { get; set; }
    }
}
