namespace FladeUp_API.Models.Teacher
{
    public class TeacherModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Image { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Status { get; set; }
    }
}
