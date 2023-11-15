using FladeUp_API.Models.User;
using Microsoft.EntityFrameworkCore;

namespace FladeUp_API.Models.Group
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FormOfStudy { get; set; }
        public List<UserPublicDataModel> Students { get; set; }

       
    }
}
