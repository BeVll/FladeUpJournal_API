using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FladeUp_API.Data.Entities.Identity
{
    public class UserAdresses
    {
        [Key]
        public int UserId { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }

        public string? MailCountry { get; set; }
        public string? MailCity { get; set; }
        public string? MailStreet { get; set; }
        public string? MailPostalCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }
}
