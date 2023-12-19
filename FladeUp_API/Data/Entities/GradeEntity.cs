namespace FladeUp_API.Data.Entities
{
    public class GradeEntity
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int StudentId { get; set; }
        public int Grade {  get; set; }
        public DateTime Created {  get; set; }
        public DateTime Updated { get; set; }
    }
}
