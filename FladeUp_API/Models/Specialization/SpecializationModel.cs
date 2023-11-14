using FladeUp_API.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Models.Specialization
{
    public class SpecializationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DepartmentModel Department { get; set; }
    }
}
