namespace FladeUp_API.Data.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int TypeId { get; set; }
        public int SubjectId { get; set; }
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public int MaximumGrade { get; set; }
        public string Image { get; set; }
        public DateTime? DeadLine { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
    }
}
