namespace FladeUp_API.Requests.Class
{
    public class ClassUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string FormOfStudy { get; set; }
        public int YearOfStart { get; set; }
        public int YearOfEnd { get; set; }
    }
}
