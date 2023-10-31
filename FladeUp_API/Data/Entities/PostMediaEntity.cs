using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_Api.Data.Entities
{
    public class PostMediaEntity
    {
        public int Id { get; set; }
        public string Path { get; set; }
        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public virtual PostEntity? Post { get; set; }
    }
}
