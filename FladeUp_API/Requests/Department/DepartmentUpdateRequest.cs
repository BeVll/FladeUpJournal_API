using FladeUp_Api.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Requests.Departament
{
    public class DepartmentUpdateRequest
    {
        public string Name { get; set; }
        public int DeanId { get; set; }
    }
}
