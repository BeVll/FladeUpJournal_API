using FladeUp_Api.Data.Entities.Identity;

namespace FladeUp_API.Models.User
{
    public class UserPublicDataModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Image { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}
