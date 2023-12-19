namespace FladeUp_API.Requests.Task
{
    public class TaskCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TypeId { get; set; }
        public int SubjectId { get; set; }
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public int MaximumGrade { get; set; }
        public DateTime? DeadLine { get; set; }
    }
}
