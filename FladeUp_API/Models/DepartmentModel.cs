using FladeUp_Api.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Models
{
    public class DepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DeanId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

    }
}
