namespace FladeUp_API.Models.Students
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Image { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Status { get; set; }
    }
}
