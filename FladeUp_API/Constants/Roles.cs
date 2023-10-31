namespace FladeUp_Api.Constants
{
    public static class Roles
    {
        public static List<string> All = new()
        {
            Admin,
            User,
            Moderator
        };
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Moderator = "Moderator";
    }
}
