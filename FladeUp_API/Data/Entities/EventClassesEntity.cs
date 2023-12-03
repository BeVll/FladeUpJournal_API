using System.ComponentModel.DataAnnotations.Schema;

namespace FladeUp_API.Data.Entities
{
    public class EventClassesEntity
    {
        public int Id { get; set; }
        [ForeignKey("Event")]
        public int EventId { get; set; }
        public virtual EventEnitity Event { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }
        public virtual ClassEntity Class { get; set; }
    }
}
