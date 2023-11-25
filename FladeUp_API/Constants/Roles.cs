namespace FladeUp_Api.Constants
{
    public static class Roles
    {
        public static List<string> All = new()
        {
            Admin,
            Student,
            Dean,
            Teacher,
            Rector
        };
        public const string Admin = "Admin";
        public const string Student = "Student";
        public const string Dean = "Dean";
        public const string Teacher = "Teacher";
        public const string Rector = "Rector";
    }
}
