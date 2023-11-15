using FladeUp_Api.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Requests.Departament
{
    public class SpecializationUpdateRequest
    {
        public string Name { get; set; }
        public int DepartamentId { get; set; }
    }
}
